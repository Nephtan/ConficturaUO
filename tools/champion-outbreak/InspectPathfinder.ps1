param([Parameter(Mandatory=$true)][string]$Client, [Parameter(Mandatory=$true)][string]$Output,
      [string]$TypeName='ClassicUO.Game.Pathfinder', [string]$MethodName='WalkTo')
$ErrorActionPreference='Stop'
$assembly=[Reflection.Assembly]::LoadFrom((Resolve-Path -LiteralPath $Client).Path)
$opcodes=@{}
foreach ($field in [Reflection.Emit.OpCodes].GetFields([Reflection.BindingFlags]'Public,Static')) {
    $opcode=$field.GetValue($null);$opcodes[[int]$opcode.Value -band 65535]=$opcode
}
$lines=[Collections.Generic.List[string]]::new()
$lines.Add('Client SHA256: '+(Get-FileHash -LiteralPath $Client -Algorithm SHA256).Hash)
$type=$assembly.GetType($TypeName,$true)
foreach ($method in $type.GetMethods([Reflection.BindingFlags]'Public,NonPublic,Static,Instance') | Where-Object { $_.Name -eq $MethodName }) {
    $lines.Add($method.ToString())
    $bytes=$method.GetMethodBody().GetILAsByteArray();$position=0
    while ($position -lt $bytes.Length) {
        $offset=$position;$value=[int]$bytes[$position];$position++
        if ($value -eq 254) { $value=65280+[int]$bytes[$position];$position++ }
        $opcode=$opcodes[$value];$operand=''
        switch ($opcode.OperandType.ToString()) {
            'InlineNone' {}
            'ShortInlineI' { $operand=[int]$bytes[$position];$position++ }
            'ShortInlineVar' { $operand=[int]$bytes[$position];$position++ }
            'InlineVar' { $operand=[BitConverter]::ToUInt16($bytes,$position);$position+=2 }
            'InlineI' { $operand=[BitConverter]::ToInt32($bytes,$position);$position+=4 }
            'InlineI8' { $operand=[BitConverter]::ToInt64($bytes,$position);$position+=8 }
            'ShortInlineR' { $operand=[BitConverter]::ToSingle($bytes,$position);$position+=4 }
            'InlineR' { $operand=[BitConverter]::ToDouble($bytes,$position);$position+=8 }
            'ShortInlineBrTarget' { $delta=[int]$bytes[$position];if ($delta -gt 127) { $delta-=256 };$position++;$operand=('IL_{0:X4}' -f ($position+$delta)) }
            'InlineBrTarget' { $delta=[BitConverter]::ToInt32($bytes,$position);$position+=4;$operand=('IL_{0:X4}' -f ($position+$delta)) }
            'InlineSwitch' {
                $count=[BitConverter]::ToInt32($bytes,$position);$position+=4
                $start=$position+4*$count;$targets=@()
                for ($index=0;$index -lt $count;$index++) { $targets+=('IL_{0:X4}' -f ($start+[BitConverter]::ToInt32($bytes,$position)));$position+=4 }
                $operand=$targets -join ','
            }
            default {
                $token=[BitConverter]::ToInt32($bytes,$position);$position+=4
                $operand=('token_{0:X8}' -f $token)
                if ($opcode.OperandType.ToString() -notin @('InlineString','InlineSig')) {
                    try { $member=$method.Module.ResolveMember($token);$operand=$member.DeclaringType.FullName+'::'+$member.ToString() } catch {}
                }
            }
        }
        $lines.Add(('IL_{0:X4}: {1} {2}' -f $offset,$opcode.Name,$operand))
    }
}
if (Test-Path -LiteralPath $Output) { throw 'Never overwrite an inspection' }
$lines | Set-Content -LiteralPath $Output -Encoding UTF8
'Read-only method IL recorded; lines: '+$lines.Count
