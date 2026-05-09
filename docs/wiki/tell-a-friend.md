# System Name: Tell A Friend Referral

### Overview
Tell A Friend is an account-tag-based referral system. It asks eligible new accounts who referred them, stores the chosen referrer on the recruit's account, and later pays referral rewards when both accounts meet the login and play-time checks.

The system is implemented by `Data/Scripts/Custom/TellAFriend/TellAFriend.cs` and the reward item base class in `Data/Scripts/Custom/TellAFriend/Rewards/StatBall.cs`. It does not use XMLSpawner and does not perform any mobile, item, or spawn math.

---

### Entry Points
* **Login Hook:** `TellAFriend.Initialize` subscribes `TAFLogin` to `EventSink.Login`, so referral prompts and reward checks run when players log in.
* **Player Command:** `[taf` opens the referral gump for eligible players. The command is registered at `AccessLevel.Player`.

An account is considered eligible to open the gump when `ac.Created >= TellAFriend.age` and it has not already set `ToldAFriend`.

---

### Referral Capture
The referral gump gives two ways to mark a referrer:

| Input Path | Code Behavior |
| :--- | :--- |
| Account name text entry | Looks up the typed account with `Accounts.GetAccount(input)`. |
| Target player character | Begins a range-10 target and accepts only `PlayerMobile` targets. |

Successful referral capture sets these tags on the recruit account:

| Account Tag | Meaning |
| :--- | :--- |
| `ToldAFriend` | The recruit has selected a referrer. |
| `Referrer` | The account name of the selected referrer. |

Rejected cases include missing account names, selecting the same account, targeting a non-player, and, when `checkIP` is true, sharing any login IP between the recruit and referrer accounts.

---

### Reward Qualification
`RewardTime` is 48 hours. Before a referral matures, the code requires:

* The recruit account to be older than `age`.
* The recruit account to still have `ToldAFriend` and `Referrer`.
* The referrer account to still exist.
* Both the recruit and referrer accounts to have `LastLogin > mindate`.
* Both accounts to have `TotalGameTime >= RewardTime`.

When those checks pass on the recruit's login, the recruit immediately receives a `ReferrerReward`. The recruit's `Referrer` and `ToldAFriend` tags are then removed.

The referrer's reward is queued through account tags:

| Account Tag | Meaning |
| :--- | :--- |
| `GotAFriend` | The referrer has one or more pending referral rewards. |
| `GotFriend` | Comma-separated account names used to count pending rewards. |

When the referrer next logs in, each comma-separated entry in `GotFriend` grants one `ReferrerReward`, then `GotAFriend` and `GotFriend` are removed.

---

### Reward Item
`ReferrerReward` inherits from `StatBall` and renames itself to:

```text
Confictura: Legend & Adventure Referral Reward Stat Increase Ball
```

The inherited `StatBall` default is item ID `6249` with `StatBonus = 10`. On use, the item must be in the user's backpack and opens a stat-selection gump. Selecting Strength, Dexterity, or Intelligence applies the stat bonus, clamps the chosen raw stat to 125, respects the character's total `StatCap`, and deletes the ball after a successful stat increase.

---

### Persistence And Serialization
Referral state is stored in account tags, which are saved with account data. The key tags are `ToldAFriend`, `Referrer`, `GotAFriend`, and `GotFriend`.

`StatBall` serializes version `0` and writes `m_StatBonus`. `ReferrerReward` calls the `StatBall` serializer through `base.Serialize(writer)`, then writes its own version `0` with no additional custom fields. Deserialization reads the same version integers in the same order.

---

### Known Rework Risks
* `mindate` and `age` are static readonly `DateTime` values calculated once at script load. They do not roll forward while the server stays up, so "last 7 days" and "account is within/older than 7 days" checks drift over uptime.
* Matured referrals check `Convert.ToBoolean(ac.GetTag("GotAFriend"))` on the recruit account before writing pending reward tags to the referrer account. That means an existing `GotFriend` list on the referrer can be overwritten instead of appended, losing pending referral rewards.

---

### Audit Notes
* Wiki was empty -> traced `TellAFriend.Initialize` -> code hooks `EventSink.Login` and runs referral checks on player login.
* Ledger claimed referral rewards for inviting players -> traced `TAFGump.OnResponse` and `TAFTarget` -> code lets eligible accounts set `ToldAFriend` and `Referrer` by account name or targeted `PlayerMobile`, with self-referral and shared-IP checks.
* Wiki had no timing details -> traced `RewardTime`, `mindate`, `age`, and `TAFLogin` -> code requires 48 hours total game time for both accounts and compares account timestamps against static seven-day cutoffs.
* Wiki had no reward detail -> traced `TAFLogin`, `ReferrerReward`, and `StatBall` -> code grants a stat ball reward to the recruit when the referral matures and queues referrer rewards through account tags.
* Wiki had no serialization detail -> traced `StatBall.Serialize` and `ReferrerReward.Serialize` -> code writes versioned item data for the reward items but uses account tags for referral state.
* Code trace revealed rework risks -> static date cutoffs drift over uptime, and pending referrer rewards can be overwritten by the recruit-account append check.
