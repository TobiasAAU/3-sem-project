/*
class characterTrait
{
    
    public characterTrait(string Name, string Image, string Desciption, int uid)
    {
        name  = Name;
        image = Image;
        description = Desciption;
        UID = uid; //1**** for misc 2**** for resouce 3**** for ability 4**** for race
        
    }
    public string name{get; set;}
    public string image{get; set;}
    public string description{get; set;}
    public int UID{get; set;}
}

class resource
{
    //todo: find ud af hvordan denne class er bygget op
}

class race : characterTrait
{
    public race(string Name, string image, string Desc, int uid, List<int> StarterRes, List<int> AmountList, List<int> StarterAbility, string PlayerRestrict ) : base(Name, image, Desc, uid)
    {
        stRes = StarterRes;
        amList = AmountList;
        stAbl = StarterAbility;
        plRest = PlayerRestrict;
    }

    public List<int> stRes {get; set;}
    public List<int> amList {get; set;}
    public List<int> stAbl {get; set;}
    public string plRest {get; set;}
}

class ability : characterTrait
{
    public ability(string Name, string image, string Desc, int uid, List<int> RaceRequirement, List<int> CareerRequirement, List<int> AbilityRequirement) : base(Name, image, Desc, uid)
    {
        raceReq = RaceRequirement;
        careerReq = CareerRequirement;
        abilReq = AbilityRequirement;
    }

    public List<int> raceReq {get; set;}
    public List<int> careerReq {get; set;}
    public List<int> abilReq {get; set;}
    public int cost {get; set;}
    
}

class career : characterTrait
{
    public career(string Name, string Image, string Desciption, int uid) : base(Name, Image, Desciption, uid)
    {

    }
}

class religion : characterTrait
{
    public religion(string Name, string Image, string Desciption, int uid) : base(Name, Image, Desciption, uid)
    {

    }
}
*/