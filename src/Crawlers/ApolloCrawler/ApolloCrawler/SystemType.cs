using System.ComponentModel;

namespace ApolloCrawler
{
    public enum SystemType
    {
        [Description("N")]
        TFSScrum,

        [Description("TFS 2010 Scrum")]
        TFS2010Scrum,

        [Description("TFS UrbanTurtle")]
        TFSUrbanTurtle,

        [Description("TFS 2010")]
        TFS2010,

        [Description("TFS 2008")]
        TFS2008,

        [Description("Pivotal Tracker")]
        Pivotal
    }
}
