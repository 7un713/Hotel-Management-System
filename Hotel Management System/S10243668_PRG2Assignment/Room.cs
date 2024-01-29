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

abstract class Room
{
    private int roomNumber;
    private string bedConfigurration;
    private double dailyRate;
    private bool isAvail;
    public int RoomNumber
    {
        get { return roomNumber; }
        set { roomNumber = value; }
    }
    public string BedConfigurration
    {
        get { return bedConfigurration; }
        set { bedConfigurration = value; }
    }
    public double DailyRate
    {
        get { return dailyRate; }
        set { dailyRate = value; }
    }
    public bool IsAvail
    {
        get { return isAvail; }
        set { isAvail = value; }
    }

    public Room() { }
    public Room(int rn, string bc, double r, bool a)
    {
        RoomNumber = rn;
        BedConfigurration = bc;
        DailyRate = r;
        IsAvail = a;
    }

    public abstract double CalculateCharge();
    public override string ToString()
    {
        return $"{RoomNumber,-16}{BedConfigurration,-24}{DailyRate,-16}{IsAvail,-13}";
    }
}