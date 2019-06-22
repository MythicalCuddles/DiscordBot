| CHANGELOG.md  |
| ------------- |

## Version 3.1.0.0

### Changes
 - [`9c74d30`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/9c74d305267a3216e4af2c80e787cfb2dcd04d23) Added SQL Command to add the table 'guilds' to the database
 - [`8dd50e7`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/8dd50e7a5f309cef085f699c4d7c511891665f95) Added SQL Command to add the table 'channels' to the database
 - [`0d3e4f8`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/0d3e4f87b3469254d210cf5c511b91765b64c4be) Added SQL Command to add the table 'bans' to the database
 - [`341ee1b`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/341ee1bd78c8a5f0d3cd5f395536f8d1e4ca7c3d) Added an additional field into the channels table to identify the guild
 - [`e680d82`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/e680d82ba638e29226bafd9359dfc1a72e0ed24f) Added channelType to the channels table in the database
 - [`81e86b7`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/81e86b797e588b9c3306065a074941dbfc3f3fa3) Added SQL Command to add channels to the database when the ready event is executed
 - [`81e86b7`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/81e86b797e588b9c3306065a074941dbfc3f3fa3) Added a check to gather information from the database to see if the channel is awardingEXP
 - [`baaed3c`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/baaed3c4b7cddc579c3a85bad49ccf556144ff25) Fixed typo in a comment
 - [`8a54a59`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/8a54a59a53533483e4f0744600d81002e90aeca2) Added ChannelUpdated Event to allow updates to the database if a channel has been updated
 - [`52e172c`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/52e172cdf8f086e545c56917a2073edff0e410c5) Added SQL Command to add channels to the database when the bot joins a new guild
 - [`f79f663`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/f79f6632dd5293a3af326f04a8be778237d4f525) Added SQL Command to add channels that are newly created that the bot can see
 - [`a9aeb0e`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/a9aeb0ed65c9fc3141936abaefd4fc5e0a5e0cee) Added Update SQL Command to update the channels table when the user toggles if the channel awards EXP
 - [`5070933`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/50709331ec4993966668648ee073865251df9fff) Changed the AwardEXP in the MessageReceived Event to use the Channel Object
 - [`68ccc23`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/68ccc2347d07ec51771e7bff8e422fd93ed8a377) Created the Channel Object to load information from the database
 - [`eb345c4`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/eb345c4b350a5139648d2d16688c61616ac80d84) Commented out Common/Channel for removal
 - [`763786f`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/763786f80e736892330f542e29b6ad60e88bbc44) Removed commented out code in UserExtensions
 - [`ac7df6d`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/ac7df6d5b384043b5c2b0445bee82d876a110dec) Changed Ready Event to call methods to issue changes to tables in the database
 - [`ac7df6d`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/ac7df6d5b384043b5c2b0445bee82d876a110dec) Added ReadyAddGuildsToDatabase to add guilds to the database when the ready event is called
 - [`ac7df6d`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/ac7df6d5b384043b5c2b0445bee82d876a110dec) Added ReadyAddChannelsToDatabase to add channels to the database when the ready event is called
 - [`ac7df6d`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/ac7df6d5b384043b5c2b0445bee82d876a110dec) Added ReadyAddUsersToDatabase to add users to the database when the ready event is called
 - [`ac7df6d`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/ac7df6d5b384043b5c2b0445bee82d876a110dec) Added ReadyAddBansToDatabase to add bans to the database when the ready event is called
 - [`3300496`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/33004967f16398705432783740baea0e891fe1b1) Added additional Guild Events
 - [`04db261`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/04db261a219cbe8388e2a0261e18637e7cbd8157) Changed dateJoined in Guilds Table from date to datetime
 - [`70718e7`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/70718e7f375ab21bcf80d3d0e94a7d772363afc5) Added InsertChannelToDB to issue an MySQL Command to the database inserting the guild channel passed
 - [`70718e7`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/70718e7f375ab21bcf80d3d0e94a7d772363afc5) Cleaned up ChannelHandler
 - [`88864c5`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/88864c5355d9184e162cc1a620b0a4e046d87b35) Added InsertGuildToDB to issue an MySQL Command to the database inserting the guild passed
 - [`88864c5`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/88864c5355d9184e162cc1a620b0a4e046d87b35) Added additional guild event methods
 - [`0895878`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/0895878fa822abd0cf3f4f2890fed021d7301c54) Cleaned up DiscordBot by moving methods to their handlers and calling methods from there such as InsertGuildToDB and InsertChannelToDB
 - [`c881e63`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/c881e633e3e05be77d5cb6ba0b3c2bbe1fca804c) Changed database engine for tables
 - [`5a6868f`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/5a6868f13657445614edeea43e57b8117a61548d) Changed params in AwardModule to read from the awards table the correct values
 - [`c776f38`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/c776f38b3fbec1b1320697b0684d475e921ea49b) Added Events to handle Guild Bans
 - [`a8d8a1a`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/a8d8a1a1a1fa41a2ffaf99a3d2c3a7c9078d4dfc) Added MySQL Commands to add and remove users from the bans table in the database when they have been banned/unbanned from a guild
 - [`463051d`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/463051d3d6763229856db3a8db00e6be2489b48e) Added MySQL Commands to update the guilds table when a guild has been updated or the Bot has been removed
 - [`83d0cb6`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/83d0cb6813e81b94d08766b3237de193e8e2d79e) Added methods to GuildHandler to handle the database commands to add, update and remove guilds from the database
 - [`e8625b4`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/e8625b4a20e7bb00e2ba7031336b4c00099dea70) Added methods to ChannelHandler to handle the database commands to add, update and remove channels from the database
 - [`7c2dc4d`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/7c2dc4de0d2266b7e573024b07dfe2e4d69398e4) Renamed variable in Channel Object from cId to channelID
 - [`60510fe`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/60510fe928ba1380f0dbbd0a00b12972d7c07352) Renamed a database field in the guilds table from enableNsfwCommands to nsfwCommandsEnabled
 - [`0cb67d7`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/0cb67d791c835bf6431c5a6e53c28312d7205b9c) Added Guild Object to handle the data read from the database
 - [`ef902e7`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/ef902e73ac7956253f2f9fb084f8a2fb8c2c0062) Updated gPrefix in DiscordBot to load from the Guild Object
 - [`b33f8cf`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/b33f8cfd979fe84cd116d6dab4218fc4ef5b675c) Added DefaultUndefinedChannelID to Configuration to set the Channel ID in Guild Object if the channel is null
 - [`596182e`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/596182ee780c6acde5bc933137f0f395bb06ce77) Added SendMessageToGuild to GuildHandler to provide information for setting up the bot in new guilds
 - [`b87264f`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/b87264f296f191b576a9e4a5f792a99a4ca6b92b) Updated files to load from Guild Object instead of Common/GuildConfiguration
 - [`e08cb84`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/e08cb84d91d60e26beda896a0b6f891bbc8f8617) Commented out GuildConfiguration to be removed
 - [`1c3be16`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/1c3be1679036746bf2a5b2f0bf7f3fa96e2ba0c6) Added Objects/Channel and Objects/Guild to the Project
 - [`a52a076`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/a52a07633007c98241c68eb850f37380006d7a6e) Cleaned up User Object
 - [`736a8a0`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/736a8a06beecf9e2e1707a9fceb64fa9417f880d) Added PROFILE_URL_ID_TAGGED to Configuration to make the profile URL configurable
 - [`87e7914`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/87e7914a62d3ac2268933f08376203c2ec5015dd) Added a link to the users online profile to their about message
 - [`87e7914`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/87e7914a62d3ac2268933f08376203c2ec5015dd) Added required EXP to level up to the about message
 - [`1a03d57`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/1a03d570d5eb62cd1b250ccbf0b5ba8fae5fcf40) Cleaned up CreateDatabaseIfNotExists in DatabaseActivity to issue one command instead of two
 - [`336efaa`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/336efaa98241cebbab85a684e3eddc032ccbf94c) Added guildsInDatabase in Ready Event to remove guilds from the database if the bot was removed from the guild whilst it was offline
 - [`8129ee4`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/8129ee43633541f99395a5d591f608b45ab3b3ef) Changed RemoveGuildFromDB from private to public
 - [`e520a51`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/e520a51698739bb39b68bbe36b59b60261f02e3b) Changed Version to 3.1.0.0
 - [`99b2f95`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/99b2f95ae82e8eab29e0cc4cdcf730dd6103ebf1) Removed Latest Version Check on Ready Event
 - [`5bf696c`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/5bf696c11ddce81aee4830c0afefc0438e6c735b) Updated Color on UserJoined Event from Red to Green
 - [`fb8a0b6`](https://github.com/MythicalCuddles/DiscordBot/pull/4/commits/fb8a0b6e0d77e9abdc157d1a304fbf3958294ada) Changed condition check for rows updated on User Joined
 
 ### Pull Requests Merged
 - [`7456019`](https://github.com/MythicalCuddles/DiscordBot/commit/7456019562745e24d10ef40b1754bc5d1a2944aa) Merge pull request [#4](https://github.com/MythicalCuddles/DiscordBot/pull/4) from MythicalCuddles/More Database Changes




## Version 3.0.0.0

### Changes
 - [`8e8f610`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/8e8f6102581789ff38a2f8ad876a5f307ada3d77) Added the user class to create user objects.
 - [`6f5b60a`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/6f5b60a48a4a409d9fe1e17f299589112f03010a) Update User.cs
 - [`2eff348`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/2eff3483fe835f09fc33bc2b0bde5434de6ec7ec) Updated User.cs
 - [`31d6974`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/31d69740a77c69093b3eff131660326d48a47e5a) Added PokemonGoFriendCode
 - [`b4d1791`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/b4d1791b5b57b80b3cb4cdfe4609b1129cb3b815) Added method call to check if the database exists
 - [`6f84d72`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/6f84d72d3ec3a2e862695ba94dfc433c232c2cd6) Added method to check for the database - currently checks the configuration file to see if the database details exist
 - [`e4a7c19`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/e4a7c19800f3988444e44381ccceafb5476f4df0) Moved database check below configuration check to ensure that the configuration file is created before using it
 - [`fd08f56`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/fd08f560dd1dfbe5bf25264cb41b30b3f1004426) Updated AwardModule to use DatabaseActivity instead
 - [`29d71da`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/29d71da23a66a84685800149f9783627d39025d7) Added Execute Command methods along with create commands to create the database and tables
 - [`e563590`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/e563590fbcfbdc35202ff1dc7db0f271462c812b) Made changes to add parameters to MySqlCommand instead of directly inserting them
 - [`052a9ee`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/052a9ee94755879e5f67f19c43a849b97cbcb179) Added DataReader to read information from the database about a user
 - [`9631680`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/96316803dd6587c1ae9db0bcfd848fb840a7cc0b) Fixed casting issue
 - [`d6ec930`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/d6ec93050ea15537d396cb2f5d915532496c7c4f) Removed byte casting and changed to GetByte method
 - [`e9928bc`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/e9928bceafcf6605dc4a7adf4bd79c3ec17d5277) Removed parameter passing for CREATE DATABASE due to errors
 - [`ba2ccb9`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/ba2ccb91539b2b7fdfeefe3b76a9855a26038424) Added startup check to ensure all users have been added to the database
 - [`e4f45fa`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/e4f45fa8c4d0c7e02f7e1ffbc8b2bf57aef399bf) Changed nullable bool to bool in User Object
 - [`18ff83c`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/18ff83ce78aa2608d0954db7cb8df7b763eba124) Changed ExecuteNonQueryCommand method return the amount of rows affected
 - [`404e92d`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/404e92d4bf027a4744bab980d16064c143e8e872) Changed #UserExtensions from Common/User to Objects/User
 - [`557ab8b`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/557ab8b6949cc666f3105a21afc20cf569c65718) Changed User Object to read and write to the database
 - [`d42e1b6`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/d42e1b6251dcf1255e32579d50db35d5a17c1f72) Updated profile module to get and update data on the database
 - [`2c706ef`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/2c706ef66a63e2d334b9861cad53abb54c007cb7) Changed variables to prevent them from being nullable
 - [`aebc52e`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/aebc52e7bd2a48286cf9f4177a053375258094b1) Added DiscordBot.Objects to use the User object instead of the User in /Common
 - [`0b5cf22`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/0b5cf22bcec5780ac1b9841d019066c1ab00d861) Updated OwnerModule to get and update data on the database
 - [`8e0d768`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/8e0d768e57584691fc687422ab39a5531104c470) Added UserUpdated Event to log changes such as a username update for the website
 - [`5e1abc5`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/5e1abc5c70391377e95545d9abd8ba6f71328c5e) Updated FunModule to read from the database
 - [`072a35e`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/072a35e74e3f84c20bc1f0a3548c95843c9d583f) Updated LeaderboardModule to read from the database
 - [`cc1a374`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/cc1a374ee3357308a7f6c7866232f88168351e0a) Updated InfoModule to read from the database
 - [`c83ccb2`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/c83ccb2b9c3fb75cf596efe6b7c9fca7136dd1c0) Updated TeamMemberModule to read from the database
 - [`30e4bf8`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/30e4bf8991cbbcaf1b8875b9e5b589160aae76c5) Updated ForceModule to get and update data on the database
 - [`33a5a73`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/33a5a733761d1d46d45a2a74e22a988988fbbbd0) Updated UserHandler to add a new user to the database and update users if they update their username or avatar
 - [`8fe7830`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/8fe78309ca238f5664375506dade39f6f03bc249) Changed the charset to allow emojis to be added to the database
 - [`373b816`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/373b8162f57e7cad1e1eb99cc97b3d59763970ed) Changed null checks to string.isnullorempty checks instead
 - [`73ebed6`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/73ebed63d8265c057f45f8bc694b3e6b9e4e4b52) Commented out Common/User.cs - file to be removed
 - [`b540e9d`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/b540e9d636ebc489967472201041677ba93177f5) Version change and adding of ValueTuple Package
 - [`01679f6`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/01679f699f3a730b65c6e136797dabf49b555320) Changed if null checks to string null checks
 - [`e744c11`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/e744c11a7c3c36c8fdfdcc03c3947050fce00a33) Added command to change Pokemon Go Friend Code, and it now appears on the about message
 - [`e488ced`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/e488cedc95661fa5dc32cb79482c90a6702e5689) Changed minecraftusername to just minecraft in ForceModule
 - [`fa87d42`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/fa87d42d555c7e6980adc22f163d68c72e79abe9) Added PokemonGoFriendCode to ForceModule
 - [`d69e52e`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/d69e52ee8ebd125aac0d2e43a8734a06cac78fff) Added GetPokemonGoFriendCode to UserExtensions
 - [`d9c15e2`](https://github.com/MythicalCuddles/DiscordBot/pull/2/commits/d9c15e25d02bdea1824196dfb4d1f11f94e6f689) Changed null check to string null check in DiscordBot to fix messages as commands issue
 - [`44dcc7f`](https://github.com/MythicalCuddles/DiscordBot/pull/3/commits/44dcc7feb4f7d944d25984f833b594328e75f623) Added AwardingEXPMentionUser to the Configuration to enable or disable mentions in the 'User Levelled Up' embed
 - [`8512950`](https://github.com/MythicalCuddles/DiscordBot/pull/3/commits/8512950166c720a4420bcb304979d603c1167980) Added AwardingEXPMentionUser to UpdateConfiguration
 - [`d43740b`](https://github.com/MythicalCuddles/DiscordBot/pull/3/commits/d43740bf460b8d0b09f16ae400c49723d21081c0) Added a check to see if the user gets mentioned when they have leveled up
 - [`bce0ba1`](https://github.com/MythicalCuddles/DiscordBot/pull/3/commits/bce0ba1e9c717e5368b421e772cbff48cf158c4f) Added a command to toggle AwardingEXPMentionUser
 - [`c88fdb0`](https://github.com/MythicalCuddles/DiscordBot/commit/c88fdb01d497f7049106c86ef07c1534dc49681d) Cleaned up the debugging code
 - [`2c62cb6`](https://github.com/MythicalCuddles/DiscordBot/commit/2c62cb68880e2773077c003acbd5f6d3aad2c67c) Fixed a database connection leak issue
 - [`b8eab58`](https://github.com/MythicalCuddles/DiscordBot/commit/b8eab58c819c07d2185a1f7e5fb131c217439f8b) Fixed an issue where the MySqlDataReader would close before the datawas saved
 
 ### Pull Requests Merged
 - [`db02c0f`](https://github.com/MythicalCuddles/DiscordBot/commit/db02c0f1d6e2c4c60dd8e13f5255395dee250fa5) Merge pull request [#2](https://github.com/MythicalCuddles/DiscordBot/pull/2) from MythicalCuddles/Database Integration and User Update
 - [`7e29e1f`](https://github.com/MythicalCuddles/DiscordBot/commit/7e29e1f57656c37c54e77f9f61f90a043cf518d8) Merge pull request [#3](https://github.com/MythicalCuddles/DiscordBot/pull/3) from MythicalCuddles/Changing EXP Message to have a toggle-able mention



## Version 2.12.3.0

### Fixed
    - Fixed an issue where awards wouldn't show if they checked for the date awarded.
    
### Please Note
You can view the [releases along with the changes by clicking here](https://github.com/MythicalCuddles/DiscordBot/releases).

## Version 2.12.2.0

### Added
    - Added "awards" command to see the specified users awards.
    - Added "prefix" command to let the user see which prefixes are available to them.
    - Added "editdatabase" command to let the Bot Owner change the database configuration.
    - Added additional variables to "editconfig" to allow the Bot Owner to change the award variables.

### Fixed
    - Fixed a typo where Quote Level Requirement in the "showconfig" command would show coins.
    - Fixed a typo where Prefix Level Requirement in the "showconfig" command would show coins.
    - Fixed a typo where RGB Level Requirement in the "showconfig" command would show coins.
    - Fixed a typo where the guild introduction message would show in coins instead of EXP.    

### Removed
    - Removed Transaction Logger as there is no need for it anymore.
    - Removed a line of code which deleted the message informing users that they have levelled up.
    
### Additional
    - Added an Award Object to make it easier to deal with user awards from the database.

## Version 2.12.1.0
    
### Fixed
    - Fixed an issue with the "leaderboard global" command.
    - Fixed the bot becoming too spammy by making the level up messages delete themselves after a couple of minutes.

## Version 2.12.0.0

### Added
    - Added "botchannel" for guild owners to get the bot channel for the guild.
    - Added Level and EXP fields into the UserJoined embed if the user exists in the database.
    - Added Level and EXP fields into the UserLeft embed if the user exists in the database.
    - Added "toggleexpawarding" from ServerOwnerModule.
    - Added "editconfig toggleexpawarding" from ConfigModule.
    - Added AwardingEXP to Channel Configurations.
    - Added GetLevel and GetEXP to UserExtensions.
    - Added AwardEXPToUser, EXPToLevelUp and AttemptLevelUp to UserExtensions.
    - Added AwardingEXPEnabled to "showconfig bot" in ShowConfigModule.
    - Added Level and EXP to User Configurations.
    - Added Level and EXP to "about" in ProfileModule.

### Changed
    - Updated the message that is posted on BotOnJoinedGuild to include the new configurable settings.
    - Changed command minlengthforcoins to minlengthforexp to fit for the EXP system.
    - Changed Quote Cost, Prefix Cost and RGB Cost in the 'showconfig bot' command to Quote Level Requirement, Prefix Level Requirement and RGB Level Requirement.
    - Changed MessageReceived Event to award EXP instead of coins now.
    - Changed "quoteprice", "prefixprice" and "rgbprice" to "quotelevel", "prefixlevel" and "rgblevel" to better fit for the leveling system.
    - Changed ReactionHandler to award EXP instead of Tokens.
    - Changed the LeaderboardModule to show Level and EXP of users instead of coins.
    - Changed "buyquote" to require a min level instead of coins.
    - Changed "customprefix" to require a min level instead of coins.
    - Changed "customrgb" to require a min level instead of coins.
    
### Fixed
    - Fixed an issue in LeaderboardModule which made a very small percentage of users unable to view the leaderboard due to their id neighbours being out of the range.

### Removed
    - Removed the Two-Factor Authentication from the initial startup.
    - Removed the requirement of Two-Factor Authentication for owner-locked commands (might come back in a future update).
    - Removed the Coins field from UserJoined.
    - Removed the Coins field from UserLeft.
    - Removed "awardcoins" from AdminModule.
    - Removed "toggleawardingcoins" from ServerOwnerModule.
    - Removed TotalCoins and TotalGuildCoins from "stats" in ModeratorModule.
    - Removed "force coins" and "force mythicaltokens" from ForceModule.
    - Removed "editconfig toggleawardingcoins" from ConfigModule.
    - Removed "editconfig toggleawardingtokens" from ConfigModule.
    - Removed AwardingCoins from Channel Configurations.
    - Removed GetCoins and GetMythicalTokens from UserExtensions.
    - Removed AwardCoinsToUser and AwardTokensToUser from UserExtensions.
    - Removed AwardingCoinsEnabled and AwardingTokensEnabled from "showconfig bot" in ShowConfigModule.
    - Removed Coins and MythicalTokens from User Configurations.
    - Removed "balance" and "givecoins" from FunModule.
    - Removed "resetallcoins" from OwnerModule.
    - Removed Coins and Mythical Tokens from "about" in ProfileModule.
    - Removed the Raid Script(s) due to the system change from coins to levelling and EXP.
    
### Additional
    - Added ConsoleHandler to hold methods to print exceptions as a log message.
    - GuildConfiguration EnsureExists() now prints as a log message instead of a console message.
    - Removed SecretKey that was used for the Two-Factor Authentication from the Configuration.
    - Configuration EnsureExists() now prints as a log message instead of a console message.
    - ChannelHandler ChannelCreated() and ChannelDestroyed() now prints as a log message instead of a console message.
    - QuoteHandler LoadAllQuotes() and EnsureExists() now prints as a log message instead of a console message.
    - StringConfiguration EnsureExists() now prints as a log message instead of a console message.
    - DiscordBot LoginAndStart() now prints as a log message instead of a console message.
    - DiscordBot ReEnterToken() now prints as a log message instead of a console message.
    - DiscordBot Ready() Event now prints as a log message instead of a console message.
    - DiscordBot Disconnected() Event now prints as a log message instead of a console message.
    - DiscordBot MessageReceived() Event now prints as a log message instead of a console message.
    - Changed the Console.WriteLine in Log() from printing logMessage and instead now prints logMessage.Message
    - TransactionLogger EnsureExists() now prints as a log message instead of a console message.
    - TransactionLogger LoadTransactions() now prints as a log message instead of a console message.
    - Removed QuoteCost, PrefixCost and RGBCost from Configuration.
    - Added QuoteLevelRequirement, PrefixLevelRequirement and RGBLevelRequirement to Configuration.
    - VoteLinkHandler LoadQuotes() and EnsureExists() now prints as a log message instead of a console message.
    - Added PrintToConsole() to Extensions to pass Log Messages through to DiscordBot.Log().
    - EmbedModule Exception now prints as a log message to the console.
    - ShowConfigModule Exception now prints as a log message to the console.
    - ProfileModule Exception(s) now prints as a log message to the console.
    - NSFWModule now prints as a log message instead of a console message.
    - Channel EnsureExists() now prints as a log message instead of a console message.
    - User CreateUserFile() now prints as a log message instead of a console message.
    - Removed SetCoinsForAll from User.cs
    - Removed AwardingCoinsEnabled and AwardingTokensEnabled from Configuration.
    - Added AwardingEXPEnabled to Configuration.

## Version 2.11.0.0

### Added
    - Added TeamMemberModule to hold commands for users with the TeamMember permission.
    - Added AwardingCoinsEnabled to check if users get awarded coins.
    - Added check in MessageReceived to check if the user gets awarded coins.
    - Added AwardingTokensEnabled to check if users get awarded tokens. 
    - Added check in ReactionAdded to check if the user gets awarded tokens.
    - Added AwardingCoinsEnabled and AwardingTokensEnabled to the command showconfig.
    - Added awardingcoins and awardingtokens to editconfig.
    - Added GitHub username to the user about card.
    - Added Instagram to the user about card.
    - Added commands to allow users to set their Instagram and GitHub username.
    - Added force commands to allow admins to force change users Instagram and GitHub username.
    - Added ProfileModule.cs.
    - Addded GitHub and Instagram to UserHandler user card for when existing user joins another server.
    - Added more custom flags to ModifyStringFlags for messages such as the welcome message.
    - Added "showconfig all" to display all the available configurations.
    - Added RGBCost to charge coins for users to issue the customrgb command.
    - Added "editconfig rgbcost" to change the price for customrgb.
    - Added RGBCost to showconfig.
    - Added GetCustomRGB as a user extension.
    - Added RequiredGuildAttribute to check if commands with the attribute are issued in the guild required.
    - [Feature in testing] Added some commands to be guild-locked commands.
    - Added parts of the raiding system (still in development, coming very soon)
    - [Feature in testing] Added "addreaction" for the bot owner (to be changed in the future) to add a reaction to a cached message.

### Changed
    - Moved Guild event(s) from DiscordBot.cs to GuildHandler.cs in Handlers.
    - Updated ReactionHandler.cs to award tokens instead of coins to the user reacting and the user posted.
    - Updated guild join message to get the username of the bot.
    - Updated DiscordBot.cs to write DiscordBot instead of MogiiBot to the console.
    - Changed a section of code which requested the developer ID from MelissaNet to load from the configuration instead.
    - Moved command globalmessage from the OwnerModule to the TeamMemberModule.
    - Changed editconfig to show Yes/No instead of True/False for some options.
    - Changed MELISSA flags to DEVELOPER flags in StringExtensions.
    - Changed DeveloperInformation in ModeratorModule stats to load developer information from the configuration instead of MelissaNet.
    - Moved user sets to ProfileModule.cs
    - Updated user sets to be within the editprofile command.
    - Moved command about to ProfileModule.cs
    - Removed alias' from quoteprice and prefixprice.
    - Changed the leaderboard layout by adding it to an embed instead of a string message.
    - Changed the NSFW rule34gamble post by adding it to an embed instead of a string message.
    - Updated the messages returned when a user successfully changes anything on their about profile card.
    
### Fixed
    - Fixed an issue where the Personal Website tag on the about embed would update to match the users input.

    
### Removed
    - Removed MelissaNet Version get during bot startup.
    - Removed MELISSA.NET.VERSION flag from StringExtensions.
    
### Additional
    - Changed AwardCoinsToPlayer() to AwardCoinsToUser().
    - Made AwardCoinsToUser() a user extension.
    
    - Updated Discord.Net and other packages to 2.0.0-beta2-00974.


## Version 2.10.1.0

### Fixed
    - Fixed an issue with the guild leaderboard.


## Version 2.10.1.0

### Changed
    - Changed the die command confirmation embed.
    - Changed the leaderboard command to default to the guild leaderboard.
    - Changed MogiiBot3.cs to DiscordBot.cs
    - Updated all instances of MogiiBot3 to DiscordBot.

### Fixed
    - Fixed a spelling mistake in global leaderboard.
    
### Removed
    - Removed unused code in MogiiBot3.cs.
    
### Additional
    Code Style
    - Changed QuoteHandler.cs to use a expression-bodied property instead of the inconsistent body style.
    - Removed redundant parentheses in GuildConfiguration.cs.
    - Removed redundant parentheses in MinPermissionsAttribute.cs.
    - Fixed built-in type references in Extensions.cs to be consistent with code style.
    - Fixed built-in type references in StringExtensions.cs to be consistent with code style.
    - Fixed built-in type references in UserExtensions.cs to be consistent with code style.

    Language Usage Opportunities
    - Changed GetUser() to use pattern matching and null propagation.
    - Changed GetTextChannel() to use pattern matching and null propagation.
    - Changed GetVoiceChannel() to use pattern matching and null propagation.
    - Changed GetGuild() to use pattern matching and null propagation.
    - Changed ListQuotes() to use a foreach-loop instead of a for-loop.
    - Changed SendVotingLinks() to use a foreach-loop instead of a for-loop.

    Common Practices and Code Improvements
    - Changed Configuration to have File field as readonly.
    - Changed StringConfiguration to have File field as readonly.
    - Changed ListRequestQuotes() to use the Any() method instead of count.
    - Changed ShowStatistics() to declare variables with more specific types.
    - Changed HelpAsync() to use String.IsNullOrEmpty instead of checking individually.
    - Changed fields in EmbedModule.cs to be readonly.
    
    Redundancies in Code
    - Removed redundant qualifiers in GuildConfigurtion.
    - Renoved redundant qualifiers in Configuration.
    - Removed redundant qualifiers in frmAuth.
    - Removed Using directives in PermissionLevel.cs.
    - Removed Using directives in StringConfiguration.cs.
    - Removed assigned value which wasn't used in any execution path in Extensions.cs.
    - Removed Using directives in frmAuth.cs.
    - Removed Using directives in ChannelHandler.cs.
    - Removed Using directives in UserHandler.cs.
    - Removed Using directives in TransactionLogger.cs.
    - Removed Using directives in AdminModule.cs.
    - Removed Using directives in ForceModule.cs.
    - Removed Using directives in SendModule.cs.
    - Removed Using directives in EmbedModule.cs.
    - Removed redundant Object.ToString() call in EmbedModule.cs.
    - Removed Using directives in ModeratorModule.cs.
    - Removed Using directives in ShowConfigModule.cs.
    - Removed redundant type cast in ShowConfigModule.cs.
    - Removed unreachable code in ShowConfigModule.cs.
    - Removed Using directives in NSFWModule.cs.
    - Removed assigned value which wasn't used in any execution path in NSFWModule.cs.
    - Removed redundant Object.ToString() call in NSFWModule.cs.
    - Removed Using directives in ConfigModule.cs.
    - Removed Using directives in OwnerModule.cs.
    - Removed Using directives in FunModule.cs.
    - Removed Using directives in HigherLowerGameModule.cs.
    - Removed Using directives in SlotsGameModule.cs.
    - Removed Using directives in HelpModule.cs.
    - Removed Using directives in InfoModule.cs.
    - Removed Using directives in LeaderboardModule.cs.
    - Fixed issue in LeaderboardModule.cs which caused expression to always be true.
    - Removed Using directives in PublicModule.cs.
    - Removed redundant Object.ToString() call in PublicModule.cs.
    - Removed Using directives in MogiiBot3.cs.
    - Removed Using directives in QuoteHandler.cs.
    - Removed Using directives in VoteLinkHandler.cs.
    - Removed Using directives in Program.cs.

    Potential Code Quality Issues
    - Changed strings in Channel.cs to verbatim strings.
    - Changed strings in Configuration.cs to verbatim strings.
    - Changed strings in GuildConfiguration.cs to verbatim strings.
    - Changed strings in StringConfiguration.cs to verbatim strings.
    - Changed strings in User.cs to verbatim strings.
    - Added ignore comment to empty catch in Extensions.cs.
    - Added check for NullReferenceException in Extensions.cs.
    - Changed strings in frmAuth.cs to verbatim strings.
    - Changed casting in ChannelHandler.cs to direct cast instead.
    - Changed strings in ChannelHandler.cs to verbatim strings.
    - Changed strings in TransactionLogger.cs to verbatim strings.
    - Replaced Enumerable.Count() in Transaction.cs with collection count property access.
    - Replaced Enumerable.Count() in AdminModule.cs with collection count property access.
    - Changed strings in EmbedModule.cs to verbatim strings.
    - Added check for NullReferenceException in EmbedModule.cs.
    - Changed ModeratorModule.cs to make some fields direct cast to avoid possible NullReferenceException.
    - Replaced Enumerable.Count() in ModeratorModule.cs with collection count property access.
    - Changed strings in NSFWModule.cs to verbatim strings.
    - Replaced Enumerable.Count() in FunModule.cs with collection count property access.
    - Replaced Enumerable.Count() in SlotsGameModule.cs with collection count property access.
    - ﻿Renamed parameter 'Context' as it hides property 'T Discord.Commands.ModuleBase<T>.Context' in LeaderboardModule.cs.
    - Added check for NullReferenceException in PublicModule.cs.
    - Changed strings in PublicModule.cs to verbatim strings.
    - Added check for NullReferenceException in ServerOwnerModule.cs.
    - Changed strings in MogiiBot3.cs to verbatim strings.
    - Changed strings in QuoteHandler.cs to verbatim strings.
    - Replaced Enumerable.Count() in QuoteHandler.cs with collection count property access.
    - Changed strings in VoteLinkHandler.cs to verbatim strings.
    - Changed strings in Program.cs to verbatim strings.


## Version 2.10.0.0

### Added
    - Added InteractiveService to ServiceProvider.
    - Added commands to do with the MogiiCraft Discord Guild into the command prefix "MogiiCraft".
    - Re-added BotChannelId to the Guild Configuration for future games and chat.
    - Re-added BotChannelId to command "showconfig" for future games and chat.
    - Added new configuration file "StringConfiguration" to allow strings to be altered whilst the bot is running.
    - Added command "showconfig strings" to display all the values in the StringConfiguration.
    - Added "WebsiteName" for Users to add the name of their website to their about.
    - Added "WebsiteUrl" for Users to add the URL to their website to their about.
    - Added Website section to command "about".
    - Added command "setwebsitename".
    - Added command "setwebsiteurl".
    - Added command "force websitename" for guild administrators to force change a users website name.
    - Added command "force websiteurl" for guild administrators to force change a users website URL.
    - Added "MythicalTokens" to the Users for future development.
    - Added MythicalTokens section to command "about".
    - Added command "force mythicaltokens" for guild administrators to force change how many Mythical Tokens the specified user has.
    - Added logging to commands "force coins" and "force mythicaltokens" to send a message to the global log channel of any changes.
    - Added GetPermissionLevel() and HasHigherPermissionLevel() to compare permission levels and restrict users with lower permissions from changing values for users with higher permissions.
    - Added a check in the "force" commands to see if the user issuing the command has a higher or equal permission level to the user specified.
    - Added GetWebsiteName(), GetWebsiteUrl() and ... to User Extensions to easily load users information.
    - Added ShowLeaderboard() method to handle the leaderboard code.
    - Added command "leaderboard global" and "leaderboard guild" with "leaderboard" automatically showing the global leaderboard. (See Changed - "Altered command "leaderboard" ...")
    - Added command "editstring" to ConfigModule to edit the StringsConfiguration.
    - Added command "editstring defaultwebsitename" to change the default name for websites in the users about.
    - Added "MogiiBot is typing..." to the command "about" (trial).

### Changed
    - Changed the User Leave message to show more information about the user.
    - Changed the User Join message to show more information if the user file exists.
    - Changed the storage location for the Configuration file(s).
    - Changed the storage location for the Channel Configuration file(s).
    - Changed the storage location for the User file(s).
    - Changed the storage location for the Guild Configuration file(s).
    - Changed the command "listquotes" to use the Paginator Service (trial).
    - Changed up command "balance" to be the main command and "wallet" as an alias.
    - Changed command "showconfig" to show a syntax for which config instead of loading all the available configs.
    - Updated command "about" to use UserExtensions.
    - Changed command "buyquote" to tell users if they don't have enough coins first to prevent the syntax from being shown if the user didn't have enough coins to buy a quote.
    - Altered command "leaderboard" to "leaderboard global" to show the global leaderboard. Command "leaderboard" will automatically post the global leaderboard.
    - Updated ChannelHandler to display new and deleted channels in embeds.
   
### Fixed
    - Fixed the syntax in the command "die".
    - Fixed an issue where the program would try to load the configuration file that doesn't exist to setup the authenticator.

### Removed
    - Removed "mogiicoin" as a balance alias from FunModule.
    - Removed command "info coins" from InfoModule.
    - Removed "Channel Message" from "editconfig".
    - Removed command "image" from FunModule.
    - Removed commands "addimage", "listimages", "editimage" and "deleteimage" from AdminModule.
    - Removed ImageHandler.cs from the project.
    - Removed ImageHandler from ReactionHandler.
    - Removed Image EnsureExists() from Program.
    - Removed MusicHandler.cs from the project. (Don't know why this wasn't removed earlier...)

### Additional
    - Cleaned up command "stats".
    - Cleaned up some users from the old "No Information Provided".
    - Added Discord.Addons.Interactive v1.0.1
    - Updated Discord.Net to v2.0.0-beta2-00964
    - Updated Discord.Net.Commands to v2.0.0-beta2-00964
    - Updated Discord.Net.Core to v2.0.0-beta2-00964
    - Updated Discord.Net.Rest to v2.0.0-beta2-00964
    - Updated Discord.Net.Webhook to v2.0.0-beta2-00964
    - Updated Discord.Net.Websocket to v2.0.0-beta2-00964
    - Updated Discord.Net.Providers.WS4Net to v2.0.0-beta2-00964
    - Updated HtmlAgilityPack to v1.8.4


## Version 2.9.0.0

### Added
    - Added command "globalmessage" for the Bot Owner to send a message to all the guilds connected to the Bot.
    - Added confirmation to command "die" for the Bot Owner.
    - Added TwoFactorAuthentication for commands "die" and "resetallcoins".
    - Added a one-time QRCode generator for the TwoFactorAuthentication, saving the secret key to be used for commands.
    - Added User Extension GetFooterText()

### Fixed
    - Fixed an issue with command "showconfig guild".

### Removed
    - Removed BotChannelID.

### Additional
    - Added GoogleAuthenticator v1.2.1
    - Updated Discord.Net to v2.0.0-beta2-00940
    - Updated Discord.Net.Commands to v2.0.0-beta2-00940
    - Updated Discord.Net.Core to v2.0.0-beta2-00940
    - Updated Discord.Net.Providers.WS4Net to v2.0.0-beta2-00940
    - Updated Discord.Net.Rest to v2.0.0-beta2-00940
    - Updated Discord.Net.Webhook to v2.0.0-beta2-00940
    - Updated Discord.Net.Websocket to v2.0.0-beta2-00940
    - Updated HtmlAgilityPack to v1.8.2
    - Updated Newtonsoft.Json to v11.0.2


## Version 2.8.1.0

### Added
	- Added "Server Time" to stats.
	- Added Activity Type ID's to editconfig.
	- Added extension method to convert Integer to ActivityType.
	- Added DatabaseHandler.cs to handle database initialisation and queries. (Still in early development)
	- Added uPrefix and gPrefix to the MessageReveived event to work with.
	- Added check to see if the user has a custom prefix for commands.

### Changed
	- Fixed command "showconfig" by adding null-coalescing operators to show which settings have yet to be set.
	- Fixed "showconfig" to show the ActivityType instead of an integer value.
	- Changed "showconfig" to show YES/NO instead of TRUE/FALSE.
	- Modified the Leave Embed to include more information about the user leaving a guild.
	- Created a variable for MinLengthForCoin to prevent configuration being loaded everytime a message is posted.
	- Changed MessageReceived message var to use pattern matching.
	- Changed "Channel Count" in stats to specify amount of text channels and voice channels.

### Fixed
	- Fixed an issue where some users wouldn't receive coins after sending a message. 🎉🎉

### Removed
	- Removed "Development For" from stats command.
	- Removed MessageHandler.cs
	- Removed MessageLogger.cs
	- Removed MessageUpdated event.
	- Removed MessageDeleted event.
	- Removed private messages being sent to the log channel.

### Additional
	- Changed formatting of CHANGELOG.md to make it appear nicer in the IDE.


## Version 2.8.0.0

### Added
	- Added Project URL to console on startup.
	- Added a die command for the Bot Owner to log the Bot out and terminate the application.
	- Added EmbedModule for Moderators+ to create and send custom embeds to specified channels.
	- EmbedModule - Added command "embed" which will display the available commands for the embed prefix.
	- EmbedModule - Added subcommand "new" which will create a new embed in memory for the user.
	- EmbedModule - Added subcommand "withtitle" which will add the title passed to the stored embed.
	- EmbedModule - Added subcommand "withdescription" which will add the description passed to the stored embed.
	- EmbedModule - Added subcommand "withfooter" which will add the footer text and URL passed to the stored embed.
	- EmbedModule - Added subcommand "withcolor" which accepts RGB values to assign to the stored embed.
	- EmbedModule - Added subcommand "send" which will send the stored embed either to the channel where the command was issued or to the channel specified.
	- EmbedModule - [Future Development to include more commands and embed configuration options.]
	- Added ShowConfigModule for Guild Moderators, Administrators, Owners and the Bot Owners to see the configuration of the Bot and Guild.
	- ShowConfigModule - Added command "showconfig" which will display both Guild and Bot Configuration Information Embeds.
	- ShowConfigModule - Added subcommand "bot" to only send the Bot Configuration Information Embed.
	- ShowConfigModule - Added subcommand "guild" to only send the Guild Configuration Information Embed.
	- Added a message to display once the Bot is added to a new guild, instructing how to configure the Bot to the guild.

### Changed
	- Changed the way the bot token is read by saving it in the configuration file instead of using MelissasCode.
	- Changed gameactivity to activity to amend for the change in Discord.Net.
	- Fixed a formatting issue in "editconfig" where "status" would appear on the same line as "gameactivity" (now activity).
	- Fixed an issue with Rule34Gamble that prevented images from being sent properly.
	- Fixed an issues where new guild configurations would assign the ID 0 to channels.
	- Fixed an issue where a guild configuration wouldn't be created once the Bot joined the guild for the first time.
	- Replaced UpdateJson to UpdateUser in User.cs, changing the method in which a User is updated.
	- Replaced UpdateJson to UpdateChannel in Channel.cs, changing the method in which a Channel Configuration is updated.
	- Replaced UpdateJson to UpdateGuild in GuildConfiguration.cs, changing the method in which a Guild Configuration is updated.
	- Replaced UpdateJson to UpdateConfiguration in Configuration.cs, changing the method in which the Bot Configuration is updated.

### Removed
	- MelissasCode has been completely removed!! 🎉🎉
	- Removed music commands and features where the user would issue a command to have a random* link posted in the chat. (*Links were added to the database by a Guild Member with the required permissions.)


## Version 2.7.1.0
	- Updated Discord.Net


## Version 2.6.3.0 / 2.7.0.0

### Added
	- Added toggleawardingcoins for server owners to toggle which channels receive coins and which don't.
	- Added channel check to startup to catch any channels created whilst the bot is offline.

### Additional
	- Added SetCoins to User Object to see if any changes are made.
	- Added Channel Object to keep track of channels awarding coins.


## Version 2.6.2.0

### Added
	- Added more emotes to the slot machines.
	- Added an algorithm to judge if a user is awarded a coin or not.
	- Added minlengthforcoins for the Bot Owner to force change one of the algorithm's variables.

### Changed
	- Changed the leaderboard to show the users current position on the board if they don't show within the top list.
	- Changed buyquote to check if quotes are enabled before allowing a user to buy a quote spot.
	- Changed the arrow in the slots from '<' to ':arrow_left:'
	- Fixed an issue with the leaderboard where it would show duplicates of users.
	- Fixed an issue with the leaderboard where it would still show bot users.

### Removed
	- Removed 'F' version info.
	- Removed GUID from stats.

### Additional
	- Changed the way commands and messages are read, ensuring that the AwardCoinsToPlayer method is called.


## Version 2.6.1.0

### Changed
	- Reverted to an older version of the leaderboard module due to issue with duplicates appearing in the list.


## Version 2.6.0.0
Many thanks to Marceline for getting MelissaNet added to MogiiBot in time for the update (Private Library)

### Added
	- Added MelissaNet.dll reference added due to the discontinue of MelissasCode.
	- Added MelissaNet version logging to startup log.
	- Added MelissaNet version added to stats.
	- Added resetallcoins for the Bot Owner to force reset all user coins.
	- Added gameactivity to editconfig to allow the Bot Owner to set the playing message and an optional twitch link.
	- Added a new alias for rule34gamble - rule34.
	- Added editiconurl for the Bot Owner to change the icons that appears in a users about embed.
	- Added a total count to the amount of respects that have been paid.

### Changed
	- Changed setaboutrgb to setcustomrgb, as it is used in more places than the about embed now.
	- Changed the way "F" looks by writing the message into an embed.
	- Changed the leaderboard to show users from all guilds.
	- Changed the leaderboard to not show any bots.
	- Changed force commands to output as an embed instead of a message.
	- Fixed an issue where the bot wouldn't load into streaming mode on restarting. (RB 149991092337639424)
	- Fixed an issue where AwardCoinsToPlayer didn't award any coins to the user. (RB 95941149587427328)
	- Fixed an issue with Rule34 where the link would not embed the image from the website. (RB 95941149587427328)
	- Fixed an issue where Rule34 would sometimes throw out a Thumbnail or Sample image. (RB 95941149587427328)

### Removed
	- Removed playing command to set the Bot playing message.
	- Removed twitch command to set the Bots status into twitch streaming status.
	- Removed a switch which checked if the user can be awarded coins for reactions.
	- Removed an error message which contained sensitive information.

### Additional
	- Cleaned up the Configuration File to make it look nice and new. (ﾉ◕ヮ◕)ﾉ*:･ﾟ✧
	- Added the Bot token to the configuration file. It will now check for a token there before asking MelissasCode for it.
	- Added offlineList Tuple to log new users and the guild they joined whilst the bot was offline.
	- Added a console message to display if no new users were found whilst the bot was offline.
	- Removed offlineUsersList due to offlineList Tuple being implemented.