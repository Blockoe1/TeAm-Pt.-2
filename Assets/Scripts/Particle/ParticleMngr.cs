using UnityEngine;

public class ParticleMngr : MonoBehaviour
{
    private static ParticleMngr inst;

    [SerializeField] private ParticleData[] _library;

    #region GS
    public static ParticleMngr Inst { get => inst; set => inst = value; }
    #endregion

    private void Awake()
    {
        inst = this;
    }

    [HideInInspector]
    public ParticleData Find(string name)
    {
        foreach (ParticleData data in _library)
            if (data.Name.Equals(name)) return data;

        Debug.LogError("Particle with the name " + name + "does not exit in the library.");
        return null;
    }

    [HideInInspector]
    public void Play(string name, Vector3 position, Quaternion rotation)
    {
        Instantiate(Find(name).Particle, position, rotation);
    }
}

[System.Serializable]
public class ParticleData
{
    [SerializeField] private string _name;
    [SerializeField] private GameObject _particle;

    #region
    public string Name { get => _name; set => _name = value; }
    public GameObject Particle { get => _particle; set => _particle = value; }
    #endregion
}

