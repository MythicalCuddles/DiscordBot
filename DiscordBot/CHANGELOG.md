| CHANGELOG.md  |
| ------------- |

## Version 2.10.1.0

### Added
    - 

### Changed
    - Changed the die command confirmation embed.
    - Changed the leaderboard command to default to the guild leaderboard.

### Fixed
    - Fixed a spelling mistake in global leaderboard.
    - 

### Removed
    - Removed unused code in MogiiBot3.cs.
    - 
    
### Other
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

### Other
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

### Other
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

### Other
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

### Backend Notes
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

### Backend Notes
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

### Backend Notes
	- Cleaned up the Configuration File to make it look nice and new. (ﾉ◕ヮ◕)ﾉ*:･ﾟ✧
	- Added the Bot token to the configuration file. It will now check for a token there before asking MelissasCode for it.
	- Added offlineList Tuple to log new users and the guild they joined whilst the bot was offline.
	- Added a console message to display if no new users were found whilst the bot was offline.
	- Removed offlineUsersList due to offlineList Tuple being implemented.