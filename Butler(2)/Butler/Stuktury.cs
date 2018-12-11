using System.Collections.Generic;

public class TablesButlerData
{
    public int nr;
    public int id;
    public string TeamHome;
    public string TeamAway;
    public string[] players;
    public List<int> scoresOR;
    public List<int> scoresCR;

    public int[] impyOR;
    public int[] impyCR;
    
    public int impsOR;
    public int impsCR;
    public int boards;
}

public class TableHeader
{
    public string[] surnames;
    public string homeTeam;
    public string awayTeam;

    public TableHeader(string[] s, string home, string away)
    {
        surnames = s;
        homeTeam = home;
        awayTeam = away;
    }
}

public class ButlerPlayer
{
    public int idx;
    public string name;
    public string team;

    public List<string> opponentsPlayer;
    public List<string> opponentsTeam;
    public List<int> imps;
    public List<int[]> impyzrozdaniami;

    public int boards;
    public int sumaImpow;
    public double butler;
    
}