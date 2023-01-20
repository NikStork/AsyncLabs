namespace AsyncPhilosopher;

public class Forks
{
    private bool isUsing;
    private int index;

    public Forks()
    {
        isUsing = false;
    }

    public bool IsUsing 
    {
        get { return isUsing; }
        set { isUsing = value; }
    }
}


