
public class Collision : System.IComparable {

    public float collisionTime {get; private set;}
    public CollisionType collisionType {get; private set;}
    
    public enum CollisionType {Bat, Crack, Break}

    public Collision(float time, CollisionType type) {
        collisionTime = time;
        collisionType = type;    
    }

    public int CompareTo(object obj) {
        if (obj == null) return 1;
        Collision other = obj as Collision;
        if (other != null) {
            return this.collisionTime.CompareTo(other.collisionTime);
        } else {
            throw new System.ArgumentException("object is not a Collision");
        }
    }

}