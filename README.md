![discordbot0](http://imgserv.mythicalcuddles.xyz/DiscordBot/DiscordBot.png)

# DiscordBot ![GitHub release](https://img.shields.io/github/release/MythicalCuddles/DiscordBot.svg) ![GitHub Release Date](https://img.shields.io/github/release-date/MythicalCuddles/DiscordBot.svg) ![GitHub last commit](https://img.shields.io/github/last-commit/MythicalCuddles/DiscordBot.svg) ![GitHub issues](https://img.shields.io/github/issues/MythicalCuddles/DiscordBot.svg) ![GitHub](https://img.shields.io/github/license/MythicalCuddles/DiscordBot.svg) [![Codacy Badge](https://api.codacy.com/project/badge/Grade/f883c061df9945ea946a769669f5fc9c)](https://www.codacy.com/app/MythicalCuddles/DiscordBot?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=MythicalCuddles/DiscordBot&amp;utm_campaign=Badge_Grade)

DiscordBot (originally called [MogiiBot3](https://github.com/MythicalCuddles/MogiiBot3)) is a C# Bot for DiscordApp which includes many different features such as allowing users to create their own profiles, earn EXP to Level Up and compete for #1 on the leaderboard, and many more features. DiscordBot uses MySQL to store user, channel and guild data to be used via commands through DiscordBot or to be viewed using the Web Application (which can be seen in the Additional Applications section below).

## Additional Applications

[DiscordBot Web](https://bot.mythicalcuddles.xyz) (not yet released) works with DiscordBot and adds new features such as viewing the leaderboard on the web, viewing user profiles, viewing guild bans and searching for users. I will be releasing this in the future for use, but for now, you may create your own by working with the data DiscordBot stores in the MySQL database.

## Table of Contents

* [DiscordBot](#discordbot-----)
  * [Additional Applications](#additional-applications)
  * [Getting Started](#getting-started)
    * [Prerequisites](#prerequisites)
    * [Additional Requirements](#additional-requirements)
  * [Setting Up & Running](#setting-up--running)
    * [MySQL Setup](#mysql-setup)
    * [DiscordBot API Token](#discordbot-api-token)
    * [DiscordBot Setup](#discordbot-setup)
  * [Additional Information](#additional-information)
    * [What I learned from this project](#what-i-learned-from-this-project)
    * [Final Notes](#final-notes)

## Getting Started

For this part, I will run through how to install onto Windows OS. This project has not been tested with any other OS and may not work with them.

### Prerequisites

* Windows Operating System
* .NET Framework 4.6 Runtime or later
* A MySQL Database

#### Additional Requirements

* AppData Edit Permissions
* Access to `root` user account on MySQL Database (or any other account with permission to optionally create a new user and create a database).

### Setting Up & Running

#### MySQL Setup
Firstly, we're going to need a MySQL database to store our data, and a user account to access the database. MySQL by default creates a user, 'root' which we will need access to in order to create our own user and database.

1. Start the MySQL Console and log in as 'root'. You may need to specify a password if you have changed it, or blank if you have not set one.
2. Next, we're going to create a database. If you've already got one, you can skip this step. Type ``CREATE DATABASE `database`;`` replacing `database` with a database name.
3. Now, we're going to create a user to access this database. If you've already got one, you can skip this step. Type ``GRANT ALL PRIVILEGES ON `database`.* TO 'user'@'host' IDENTIFIED BY 'password';`` replacing `database` with the database we created in step 2, `user` with a username, `host` with the hostname (such as localhost) and `password` with a strong password.

That should be the database ready to go. DiscordBot runs multiple SQL Scripts on startup to create tables within the database so we won't need to worry about creating them here.

#### DiscordBot API Token
In order to get a bot account, we're going to need to head to the [Developer Portal on DiscordApp](https://discordapp.com/developers) and login. Once logged in:

1. Create a New Application and give it a name. Don't worry about the name as you can change it later if you wish.
2. In the Application Settings, head to `Bot`, and click `Add Bot`. This will add a Bot to your application and create a `TOKEN` in the process. It is important that you keep that token a secret and do not share it! We'll need it later to connect DiscordBot to your Bot Application.
3. Next, we're going to need to get your bot connected to your guild. Head back to `General Information` and copy the `CLIENT ID`.
4. Head to the [Permission Calculator](https://discordapi.com/permissions.html#8) and insert your `CLIENT ID` into the Client ID textbox. This will generate a link below which you can use to invite the bot to your guild.
5. Click the link, and select the guild you wish to invite the bot into. You may require the `Manage Guild` permission to invite the bot to a guild you do not own.

From here, you can head back to your Bot Application and move onto the next steps in setting up DiscordBot.

Please Note: In order for DiscordBot to completely work, it needs the `Administrator` Permission or the `Manage Bans` Permission along with all the other general permissions. This is due to DiscordBot storing information in the database such as the guild bans to be used on the Web Application.

#### DiscordBot Setup
To install DiscordBot, we're going to need the executable files.

1. Head over to [releases and download the latest release](https://github.com/MythicalCuddles/DiscordBot/releases).
2. Extract the files into their own directory and head into it, followed by `/Release/`.
3. Run the executable `DiscordBot.exe`. From here, we have the application running, now we just need to set it up.
4. Read through the message prompts, completing the information as shown.

```TEXT
When asked for Bot Token: Head back to the Bot Application on the Developer Portal, and copy the `TOKEN` in the `Bot` setting and paste it into the console.
Console should clear after the Bot Token has been provided.

When asked for Database Hostname: This is the host of your database, as specified when setting up MySQL and the `host`.
Typically, this would be `localhost` or `127.0.0.1`.

When asked for Database Port: This is the port your MySQL Server is hosted on.
By default, this is port 3306. You can leave it blank if you don't know or haven't changed it.

When asked for Database Username: This is the user we created in setting up MySQL when we specified `user`.
By default, this is usually `root`, but you should change it to a user account you created. (Unsure why? You might want to look into MySQL Security and Root User Account)

When asked for Database Password: This is the password we associated with the account we created and specified in `password` when setting up MySQL.
You should not leave this field blank.

When asked for Database Name: This is the database we created while setting up MySQL. This should be what you specified as `database` in the setup phase.
If you haven't created a database, DiscordBot will automatically default to the database `discordbot` and attempt to create it.
```

DiscordBot should then attempt to create all the tables required to store information for the bot. After this process is completed, DiscordBot will start to run. Congratulations, you have successfully setup DiscordBot!

---
## Additional Information

### What I learned from this project

* Using libraries to interface with APIs
* Using JSON to store data
* Creating files to write and read from
* Using GitHub as a version control platform to manage the project
* Creating extension methods
* Using attributes to manage permissions and required conditions
* Using a module base to manage bot commands
* Using async and Task to allow code to execute and return
* Using branches and push requests to easily manage what is being worked on and test each new feature separately
* MySQL Databases to store data
* MySQL Commands to create a database and tables, and populate the tables with data which needs to be updated constantly
* Creating objects to easily manage data

### Final Notes

* This project is still being added to as I continue to learn and think of new ideas and how to approach and execute them.

---

<p align="center">
  <a href="https://mythicalcuddles.xyz"><img src="https://i.imgur.com/f45s9EN.png"></a>
  <a href="https://www.paypal.me/mythicalcuddles"><img src="https://img.shields.io/badge/Support%20the%20Developer-Donate%20via%20PayPal-ffa329.svg"></a>
  <a href="https://www.patreon.com/mythicalcuddles"><img src="https://img.shields.io/badge/Support%20the%20Developer-Become%20a%20Patreon-ffa329.svg"></a>
</p>
