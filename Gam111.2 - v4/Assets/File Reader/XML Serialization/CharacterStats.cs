using System.Xml;
using System.Xml.Serialization;

public class CharacterStats {
    [XmlAttribute("name")]
    public string name;
    public int maxHealth;
    public int power;
    public int energy;
}