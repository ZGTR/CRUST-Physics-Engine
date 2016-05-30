using Microsoft.Xna.Framework;

namespace CRUSTEngine.ProjectEngines
{
    public interface IUpdatableComponent
    {
        new void Update(GameTime gameTime);
        new void Draw(GameTime gameTime);
    }
}
