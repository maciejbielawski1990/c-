public class Player
{
    public string nazwisko;
    public string team_your;
    public string team_against;
};

public class Butler_S
{
    public Player player_name;
    public int[] imp;
    public string[] team_against;
    public int boards;
    public double butler;
};

public class Settings
{
    public string serwer;
    public int rounds_count;
    public string LinksFilename;
    public int boards;
};

public class TablesData
{
    public int nr;
    public string TeamHome;
    public string TeamAway;
    public string[] players;
    public int[] scores;
    public int impsNS;
}