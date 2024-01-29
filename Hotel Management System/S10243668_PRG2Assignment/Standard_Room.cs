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

class Standard_Room : Room
{
    private bool requireWifi;
    private bool requireBreakfast;

    public bool RequireWifi
    {
        get { return requireWifi; }
        set { requireWifi = value; }
    }
    public bool RequireBreakfast
    {
        get { return requireBreakfast; }
        set { requireBreakfast = value; }
    }
    public Standard_Room() { }
    public Standard_Room(int rn, string bc, double r, bool a) : base(rn, bc, r, a) { }
    public override double CalculateCharge()
    {
        double cost = base.DailyRate;
        if (requireBreakfast == true)
            cost += 20;
        if (requireWifi == true)
            cost += 10;
        return cost;
    }

    public override string ToString()
    {
        return base.ToString() + '\t' + RequireWifi + "\t" + RequireBreakfast;
    }
}