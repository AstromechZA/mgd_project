
using System;
using System.Collections;

public class BasicHashSet
{
	private Hashtable data;
	
	public BasicHashSet ()
	{
		data = new Hashtable();
	}
	
	public void Add(Object o) {
		data[o] = true;
		
	}
	
	public bool Contains(Object o) {
		return data.Contains(o);
	}
	
	public void Remove(Object o) {
		data.Remove(o);
	}	
	
	public ArrayList ToArrayList() {
		return new ArrayList(data.Keys);
	}
	
	
}

