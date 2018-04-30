using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

[XmlRoot("CharacterStatsCollections")]
public class CharacterStatsContainer {
    [XmlArray("CharacterStatsArray")]
    [XmlArrayItem("CharacterStats")]
    public List<CharacterStats> characterStatsList = new List<CharacterStats>();

    public void Save(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterStatsContainer));
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, this);
        stream.Close();
    }

    public CharacterStatsContainer Load(string path) {
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterStatsContainer));
        FileStream stream = new FileStream(path, FileMode.Open);
        CharacterStatsContainer characterStatsContainer = (CharacterStatsContainer)serializer.Deserialize(stream);
        stream.Close();
        return characterStatsContainer;
    }
}

/*
<xml...>
    <CharacterStatsCollection>
        <CharacterStatsArray>
            <CharacterStats name = "Character1">
                <speed>6</speed>
            </CharacterStats>
        </CharacterStatsArray>
    </CharacterStatsCollection>
</>
*/
