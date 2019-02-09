namespace DiscordBot.Objects
{
    public class User
    {
        public int? Level { get; set; } = 0;
        public int? EXP { get; set; } = 0;
        
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Pronouns { get; set; }
        public string About { get; set; }
        
        public string CustomPrefix { get; set; }
        
        public byte? AboutR { get; set; } = 140;
        public byte? AboutG { get; set; } = 90;
        public byte? AboutB { get; set; } = 210;

        public bool? TeamMember { get; set; } = false;
        public string EmbedAuthorBuilderIconUrl { get; set; }
        public string EmbedFooterBuilderIconUrl { get; set; }
        public string FooterText { get; set; }

        public string MinecraftUsername { get; set; }
        public string SnapchatUsername { get; set; }
        public string InstagramUsername { get; set; }
        public string GitHubUsername { get; set; }
        public string PokemonGoFriendCode { get; set; } 
        public string WebsiteName { get; set; }
        public string WebsiteUrl { get; set; }

        public bool? IsBotIgnoringUser { get; set; } = false;
        
        public User() { }

        public User(int? level = null,
            int? exp = null,
            string name = null,
            string gender = null,
            string pronouns = null,
            string about = null,
            string customPrefix = null,
            byte? aboutR = null,
            byte? aboutG = null,
            byte? aboutB = null,
            bool? teamMember = null,
            string embedAuthorBuilderIconUrl = null,
            string embedFooterBuilderIconUrl = null,
            string footerText = null,
            string minecraftUsername = null,
            string snapchatUsername = null,
            string instagramUsername = null,
            string gitHubUsername = null,
            string pokemonGoFriendCode = null,
            string websiteName = null,
            string websiteUrl = null,
            bool? isBotIgnoring = null)
        {
            Level = level;
            EXP = exp;
            Name = name;
            Gender = gender;
            Pronouns = pronouns;
            About = about;
            CustomPrefix = customPrefix;
            AboutR = aboutR ?? AboutR;
            AboutG = aboutG ?? AboutG;
            AboutB = aboutB ?? AboutB;
            TeamMember = teamMember ?? TeamMember;
            EmbedAuthorBuilderIconUrl = embedAuthorBuilderIconUrl;
            EmbedFooterBuilderIconUrl = embedFooterBuilderIconUrl;
            FooterText = footerText;
            MinecraftUsername = minecraftUsername;
            SnapchatUsername = snapchatUsername;
            InstagramUsername = instagramUsername;
            GitHubUsername = gitHubUsername;
            PokemonGoFriendCode = pokemonGoFriendCode;
            WebsiteName = websiteName;
            WebsiteUrl = websiteUrl;
            IsBotIgnoringUser = isBotIgnoring ?? IsBotIgnoringUser;
        }
    }
}
