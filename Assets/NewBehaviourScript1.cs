using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Speed
{
	public float speed, slowMembership=0, midMembership=0,fastMembership=0;
	Function function;
	public Speed(float speed,int flag)
	{
		this.speed = speed;
		if (0 > speed)
			this.speed = -1 * this.speed;
		if(this.speed<= 2)
			Slow(this.speed,flag);
		if (1 <=this.speed && this.speed <= 4)
			Mid(this.speed);
		if (3 <=this.speed)
			Fast(this.speed);
	}     
	private void Slow(float speed,int flag)
	{
		if (speed < 0) {
			if (flag==0) {
				//slowMembership = 0;
				flag++;
			} else
				slowMembership = 1;
		}
		else {
			function = new Function(-2, 0, 2, speed);
			slowMembership = function.membership;
		}
	}
	private void Mid(float location)
	{
		function = new Function(1, 2.5f, 4, speed);
		midMembership = function.membership;
	}
	private void Fast(float speed)
	{
		if (speed > 3)
			this.midMembership = 1;
		else{
			function = new Function(3, 5, 7, speed);
			fastMembership = function.membership;
		}
	}
}
class Slope 
{
	public float slope,x1=0,x2=0,x3=0,x4=0,mShip1=0,mShip2=0,mShip3=0;
	int m1=0,b1=15,
		a2=10,m2=15,b2=20,
		a3=15,m3=30,b3=30;
	public Slope(float closeMembership,float midMembership,float farMembership,Speed speed)
	{
		SmallSlope(closeMembership,speed.slowMembership,speed.midMembership);
		MidleSlope(midMembership,speed.slowMembership,speed.midMembership,speed.fastMembership);
		BigSlope(farMembership,speed.midMembership,speed.fastMembership);
		Defuzzification();
	}     
	private void SmallSlope(float lMembership,float s1Membership,float s2Membership)
	{
		if (lMembership >= s1Membership) {
			mShip1 = s1Membership;
			if (s1Membership <= s2Membership)
				x1 = s1Membership * (m2 - a2) + a2;
			else
				x1 = -1 * (s1Membership * (b1 - m1) - b1);
		} else {
			mShip1 = lMembership;
			if (lMembership <= s2Membership)
				x1 = lMembership * (m2 - a2) + a2;
			else
				x1 = -1 * (lMembership * (b1 - m1) - b1);
		}
	}
	private void MidleSlope(float lMembership,float s1Membership,float s2Membership,float s3Membership)
	{
		if (lMembership >= s2Membership) {
			mShip2 = s2Membership;
			if (s1Membership <= s2Membership)
				x2 = s2Membership * (m2 - a2) + a2;
			else
				x2 = -1 * (s2Membership * (b1 - m1) - b1);
			if (s3Membership <= s2Membership)
				x3 = -1 * (s2Membership * (b2 - m2) - b2);
			else
				x3 = s2Membership * (m3 - a3) + a3;	
		} else {
			mShip2 = lMembership;
			if (s1Membership <= lMembership)
				x2 = lMembership * (m2 - a2) + a2;
			else
				x2 = -1 * (lMembership * (b1 - m1) - b1);
			if (s3Membership <= lMembership)
				x3 = -1 * (lMembership * (b2 - m2) - b2);
			else
				x3 = lMembership * (m3 - a3) + a3;	
		}
	}
	private void BigSlope(float lMembership,float s2Membership,float s3Membership)
	{
		if (lMembership >= s3Membership) {
			mShip3 = s3Membership;
			if (s2Membership <= s3Membership)
				x4 = s3Membership * (m3 - a3) + a3;
			else
				x4 = -1 * (s3Membership * (b2 - m2) - b2);
		} else {
			mShip3 = lMembership;
			if (lMembership <= s2Membership)
				x4 = lMembership * (m2 - a2) + a2;
			else
				x4 = -1 * (lMembership * (b3 - m3) - b3);
		}
	}
	private void Defuzzification()
	{
		float number;
		slope = mShip1 * ((x1 * (x1 + 1)) / 2) + mShip2 * ((((x3 * (x3 + 1)) / 2) - ((x2 * (x2 + 1)) / 2))+2) + mShip3 * (((30 * 31) / 2) - ((x4 * (x4 + 1)) / 2));
		number=(mShip1*x1+ mShip2*(x3-x2+1)+mShip3*(30-x4+1));
		if(number!=0)
			slope=slope/number;
		else
			slope=0;
	}
}
class Location
{
	public float location, closeMembership=0, midMembership=0,farMembership=0;
	Function function;
	public enum variable {negative, pozitive};
	public variable type;
	public Location(float location)
	{
		this.location = location;
		if (0 <= location) 
			this.type = variable.pozitive;
		else{
			this.type = variable.negative;
			this.location = -1 * this.location;
		}
		if(this.location<= 2)
			Yakin(this.location);
		if (1 <=this.location && this.location <= 4)
			Orta(this.location);
		if (3 <=this.location)
			Uzak(this.location);
	}     
	private void Yakin(float location)
	{
		if (location < 0)
			closeMembership = 1;
		else {
			function = new Function(-2, 0, 2, location);
			closeMembership = function.membership;
		}
	}
	private void Orta(float location)
	{
			function = new Function(1, 2.5f, 4, location);
			midMembership = function.membership;
	}
	private void Uzak(float location)
	{
		if (location > 5)
			this.midMembership = 1;
		else{
			function = new Function(3, 5, 7, location);
			midMembership = function.membership;
		}
	}
}
class Function
{
	public float membership;
	public Function(float a,float b,float c,float x)
	{
		if (x <= a)
			this.membership = 0;
		else if (a < x && x <= b)
			this.membership = (x - a) / (b - a);
		else if (b < x && x <= c)
			this.membership = (c - x) / (c - b);
		else if (x > c)
			this.membership = 0;
	}
}
public class NewBehaviourScript1 : MonoBehaviour {
	GameObject ball;
	GameObject platform;
    Slider xSlider,ySlider;
	Rigidbody rb;
	int flag = 0;

	// Use this for initialization
	void Start() {													
		ball = GameObject.Find ("Sphere");																												Time.timeScale = 5.0f;
		/*ball.transform.position.x = 0;
		ball.transform.position.z = 0;
		ball.transform.position.y = 4;	*/
		platform = GameObject.Find ("Cube");
        xSlider = GameObject.Find("XSlider").GetComponent<Slider>();
        ySlider = GameObject.Find("YSlider").GetComponent<Slider>();
        rb = ball.GetComponent<Rigidbody> ();
	}
	Slope xslope;
	Slope zslope;
	// Update is called once per frame
	void Update () {
		Location x = new Location((float)ball.transform.position.x);
		Location z = new Location((float)ball.transform.position.z);
		Speed xSpeed = new Speed ((float)rb.velocity.x,flag);
		Speed zSpeed = new Speed ((float)rb.velocity.z,flag);
		xslope = new Slope (z.closeMembership, z.midMembership, z.farMembership,xSpeed);
		zslope = new Slope (x.closeMembership, x.midMembership, x.farMembership,zSpeed);
		if (z.type == Location.variable.pozitive)
			xslope.slope *= -1;
		if (x.type == Location.variable.negative)
			zslope.slope *= -1;
}
	void OnCollisionStay(Collision col)
	{
		platform.transform.parent.rotation=Quaternion.Euler(xslope.slope,0,zslope.slope);
	}
    public void ListenSlider()
    {
        rb.useGravity = false;
        ball.transform.position = new Vector3(xSlider.value, 2f, ySlider.value);
    }
    public void PlaySimulation()
    {
        rb.useGravity = true;
    }
} 
