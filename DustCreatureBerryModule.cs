namespace Celeste.Mod.DustCreatureBerry;

public class DustCreatureBerryModule : EverestModule
{
    public override void Load()
    {
        On.Celeste.Strawberry.ctor += Strawberry_ctor;
        On.Celeste.Strawberry.Update += Strawberry_Update;
    }
    Dictionary<Strawberry, int> dict = new();
    private void Strawberry_Update(On.Celeste.Strawberry.orig_Update orig, Strawberry self)
    {
        orig.Invoke(self);
        if(!dict.ContainsKey(self))
            dict.Add(self, 0);
        if (self.Follower.Leader != null)
            dict[self]++;
    }

    private void Strawberry_ctor(On.Celeste.Strawberry.orig_ctor orig, Strawberry self, EntityData data, Vector2 offset, EntityID gid)
    {
        orig.Invoke(self, data, offset, gid);
        self.Add(new PlayerCollider(player => DCBCollision(player, self)));
    }

    void DCBCollision(Player madline, Strawberry berry)
    {
        if (dict.ContainsKey(berry) && dict[berry] > 20)
            madline.Die(madline.Center - berry.Center);
    }
    public override void Unload()
    {
        On.Celeste.Strawberry.ctor -= Strawberry_ctor;
        On.Celeste.Strawberry.Update -= Strawberry_Update;
    }
}