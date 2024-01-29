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

class Deluxe_Room : Room
{
    private bool additionalBed;
    public bool AdditionalBed
    {
        get { return additionalBed; }
        set { additionalBed = value; }
    }

    public Deluxe_Room() { }
    public Deluxe_Room(int rn, string bc, double r, bool a) : base(rn, bc, r, a) { }
    public override double CalculateCharge()
    {
        double cost = base.DailyRate;
        if (AdditionalBed == true)
            cost += 25;
        return cost;
    }

    public override string ToString()
    {
        return base.ToString() + "\t" + AdditionalBed;
    }
}
