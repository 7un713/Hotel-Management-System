//========================================================== 
// Student Number : S10243668
// Student Name : Law Jun Jie
// Student Number : S10242004
// Student Name : John Wong
//========================================================== 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Membership
{
    private string status;
    private int points;
    public string Status
    {
        get { return status; }
        set { status = value; }
    }
    public int Points
    {
        get { return points; }
        set { points = value; }
    }
    public Membership() { }
    public Membership(string s, int p)
    {
        Status = s;
        Points = p;
    }
    public void EarnPoints(double amount)
    {
        double pointearned = amount / 10;
        Points += Convert.ToInt32(pointearned);
    }
    public bool RedeemPoints(int p)
    {
        if ((Status == "Silver" || Status == "Gold") && Points >= p)
        {
            return true;
        }
        else 
            return false;
    }
    public override string ToString()
    {
        return $"{Status,-10}  {Points}";
    }
}