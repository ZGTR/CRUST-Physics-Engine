namespace CRUSTEngine.ProjectEngines.PhysicsEngine
{
    public class MaterialManager
    {
        public static Material GetMaterialByIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return Material.Rubber;
                case 1:
                    return Material.Wood;
                case 2:
                    return Material.Glass;
                case 3:
                    return Material.Steel;
                case 4:
                    return Material.Concrete;
                case 5:
                    return Material.Ice;
                default:
                    return Material.Steel;
            }
        }
    }
}
