using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Server;
using Server.Mobiles;

// Reads compiled IL, so aliases, multiline assignments and property syntax cannot
// hide direct calls to the skill mutation sinks. Reflection is reported separately.
internal static class SkillCapMutationInventory
{
    public static void Export(string directory)
    {
        Dictionary<short, OpCode> codes = new Dictionary<short, OpCode>();
        foreach (FieldInfo field in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            OpCode code = (OpCode)field.GetValue(null);
            codes[code.Value] = code;
        }
        List<string> writes = new List<string>();
        List<string> reflection = new List<string>();
        List<string> npc = new List<string>();
        List<Type> regionTypes = new List<Type>();
        Dictionary<MethodBase, List<MethodBase>> regionCalls = new Dictionary<MethodBase, List<MethodBase>>();
        int methodCount = 0;
        int bodyCount = 0;
        int failures = 0;
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
        foreach (Assembly assembly in new Assembly[] { typeof(Skill).Assembly, typeof(PlayerMobile).Assembly })
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(Region).IsAssignableFrom(type))
                    regionTypes.Add(type);
                List<MethodBase> methods = new List<MethodBase>();
                methods.AddRange(type.GetMethods(flags));
                methods.AddRange(type.GetConstructors(flags));
                if (type.TypeInitializer != null && !methods.Contains(type.TypeInitializer))
                    methods.Add(type.TypeInitializer);
                foreach (MethodBase method in methods)
                {
                    ++methodCount;
                    MethodBody body = method.GetMethodBody();
                    if (body == null)
                        continue;
                    ++bodyCount;
                    byte[] il = body.GetILAsByteArray();
                    int position = 0;
                    while (position < il.Length)
                    {
                        int offset = position;
                        short value = il[position++];
                        if (value == 0xFE)
                            value = (short)(0xFE00 | il[position++]);
                        OpCode code = codes[value];
                        if (code.OperandType == OperandType.InlineMethod || code.OperandType == OperandType.InlineField)
                        {
                            try
                            {
                                MemberInfo target = method.Module.ResolveMember(BitConverter.ToInt32(il, position),
                                    type.IsGenericType ? type.GetGenericArguments() : null,
                                    method.IsGenericMethod ? method.GetGenericArguments() : null);
                                string owner = target.DeclaringType.FullName;
                                string name = target.Name;
                                bool skill = (owner == "Server.Skill" && (name.StartsWith("set_") || name == ".ctor"))
                                    || (owner == "Server.Skills" && (name == "set_Cap" || name == "set_Total" || name == ".ctor"))
                                    || (owner == "Server.Mobile" && (name == "set_SkillsCap" || name == "set_Player"))
                                    || (owner == "Server.Mobiles.PlayerMobile" && (name == "set_Skill_Start" || name == "set_Skill_Boost" || name == "set_Skill_Ether"));
                                bool store = code == OpCodes.Stfld || code == OpCodes.Stsfld;
                                bool fieldWrite = store && ((owner == "Server.Skill" && (name == "m_Base" || name == "m_Cap"))
                                    || (owner == "Server.Skills" && (name == "m_Total" || name == "m_Cap" || name == "m_Skills"))
                                    || (owner == "Server.Mobile" && (name == "m_Skills" || name == "m_Player"))
                                    || (owner == "Server.Mobiles.PlayerMobile" && (name == "SkillStart" || name == "SkillBoost" || name == "SkillEther")));
                                string row = Csv(assembly.GetName().Name) + "," + Csv(type.FullName + "::" + method.ToString())
                                    + "," + Csv("IL_" + offset.ToString("X4")) + "," + Csv(code.Name)
                                    + "," + Csv(owner + "::" + target.ToString());
                                if (skill || fieldWrite)
                                    writes.Add(row);
                                if ((owner == "System.Reflection.PropertyInfo" || owner == "System.Reflection.FieldInfo") && name == "SetValue")
                                    reflection.Add(row);
                                if (owner == "Server.Mobiles.BaseCreature" && name == "SetSkill")
                                    npc.Add(row);
                                if (method.Name == "OnEnter" && typeof(Region).IsAssignableFrom(type)
                                    && target is MethodBase && (name == "OnEnter" || name == "SkillVerification"))
                                {
                                    if (!regionCalls.ContainsKey(method))
                                        regionCalls[method] = new List<MethodBase>();
                                    regionCalls[method].Add((MethodBase)target);
                                }
                            }
                            catch (Exception exception)
                            {
                                ++failures;
                                Console.WriteLine("UNRESOLVED IL: " + type.FullName + "::" + method.Name + " " + exception.Message);
                            }
                        }
                        switch (code.OperandType)
                        {
                            case OperandType.InlineNone: break;
                            case OperandType.ShortInlineBrTarget:
                            case OperandType.ShortInlineI:
                            case OperandType.ShortInlineVar: position += 1; break;
                            case OperandType.InlineVar: position += 2; break;
                            case OperandType.InlineI8:
                            case OperandType.InlineR: position += 8; break;
                            case OperandType.InlineSwitch:
                                position += 4 + 4 * BitConverter.ToInt32(il, position); break;
                            default: position += 4; break;
                        }
                    }
                }
            }
        }
        Write(directory, "compiled-mutation-calls.csv", writes);
        Write(directory, "reflection-writes.csv", reflection);
        Write(directory, "npc-skill-setter-calls.csv", npc);
        List<string> regions = new List<string>();
        foreach (Type type in regionTypes)
        {
            MethodInfo handler = type.GetMethod("OnEnter", new Type[] { typeof(Mobile) });
            // Inherited MethodInfo instances have a different ReflectedType and do
            // not compare equal to the declared-method keys used above.
            handler = handler.DeclaringType.GetMethod("OnEnter", new Type[] { typeof(Mobile) });
            regions.Add(Csv(type.FullName) + "," + Csv(handler.DeclaringType.FullName + "::OnEnter")
                + "," + Csv(Verifies(handler, regionCalls, new HashSet<MethodBase>()).ToString()));
        }
        regions.Sort(StringComparer.Ordinal);
        regions.Insert(0, "RegionType,EffectiveHandler,CallsSkillVerification");
        File.WriteAllLines(Path.Combine(directory, "region-entry-calls.csv"), regions.ToArray(), new UTF8Encoding(false));
        Console.WriteLine("IL INVENTORY: methods=" + methodCount + "; bodies=" + bodyCount + "; direct mutation references="
            + writes.Count + "; reflection writes=" + reflection.Count + "; NPC SetSkill calls=" + npc.Count + "; unresolved=" + failures);
        if (failures != 0)
            throw new Exception("IL inventory contains unresolved operands.");
    }

    private static bool Verifies(MethodBase method, Dictionary<MethodBase, List<MethodBase>> calls, HashSet<MethodBase> seen)
    {
        if (!seen.Add(method))
            return false;
        if (method.DeclaringType == typeof(PlayerMobile) && method.Name == "SkillVerification")
            return true;
        List<MethodBase> targets;
        if (calls.TryGetValue(method, out targets))
            foreach (MethodBase target in targets)
                if (Verifies(target, calls, seen))
                    return true;
        return false;
    }

    private static string Csv(string value) { return "\"" + value.Replace("\"", "\"\"") + "\""; }

    private static void Write(string directory, string name, List<string> rows)
    {
        rows.Sort(StringComparer.Ordinal);
        List<string> output = new List<string>(rows);
        output.Insert(0, "Assembly,Caller,Offset,Opcode,Target");
        File.WriteAllLines(Path.Combine(directory, name), output.ToArray(), new UTF8Encoding(false));
    }
}
