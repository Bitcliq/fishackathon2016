package md563e4ca2e9203bdd0bf4236d5955e438e;


public class EditorCustomRenderer
	extends md5530bd51e982e6e7b340b73e88efe666e.EditorRenderer
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("PinFixed.Droid.EditorCustomRenderer, PinFixed.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", EditorCustomRenderer.class, __md_methods);
	}


	public EditorCustomRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == EditorCustomRenderer.class)
			mono.android.TypeManager.Activate ("PinFixed.Droid.EditorCustomRenderer, PinFixed.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public EditorCustomRenderer (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == EditorCustomRenderer.class)
			mono.android.TypeManager.Activate ("PinFixed.Droid.EditorCustomRenderer, PinFixed.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public EditorCustomRenderer (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == EditorCustomRenderer.class)
			mono.android.TypeManager.Activate ("PinFixed.Droid.EditorCustomRenderer, PinFixed.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
