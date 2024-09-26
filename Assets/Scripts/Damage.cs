public class Damage
{
    public Damage(float fire, float cold, float lightning, float _void, float phys)
    {
        this._fire = fire;
        this._cold = cold;
        this._lightning = lightning;
        this._void = _void;
        this._physical = phys;
    }
    public float _fire;
    public float _cold;
    public float _lightning;
    public float _void;
    public float _physical;
    public override string ToString()
    {
        return $"{typeof(Damage)} fire = {_fire} cold = {_cold} light = {_lightning} void = {_void} phys = {_physical}";
    }
}
