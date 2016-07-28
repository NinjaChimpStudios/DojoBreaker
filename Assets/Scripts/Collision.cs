
public struct Collision : System.IComparable {

    public float collisionTime {get; private set;}
    public int collisionType {get; private set;}
    
    public enum collisionTypes {Bat, Crack, Break}

    public Collision(float time, int type) {
        collisionTime = time;
        collisionType = type;    
    }

    public int CompareTo(object obj) {
        if (obj != null && obj.GetType() == typeof(Collision)) {
            Collision other = (Collision)obj;
            if (this.collisionTime != other.collisionTime) {
                // could return this - other, but we get into rounding issues with casting to int
                return (this.collisionTime < other.collisionTime) ? -1 : 1;
            }
        }
        return 0;
    }

}