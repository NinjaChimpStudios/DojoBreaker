
public class Collision : System.IComparable {

    public float collisionTime {get; private set;}
    public int collisionType {get; private set;}
    
    public enum collisionTypes {Bat, Crack, Break}

    public Collision(float time, int type) {
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