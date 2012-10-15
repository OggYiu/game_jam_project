//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// UI Atlas contains a collection of sprites inside one large texture atlas.
/// </summary>

[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : UIBaseAtlas
{
	/// <summary>
	/// Mark all widgets associated with this atlas as having changed.
	/// </summary>

	public void MarkAsDirty ()
	{
		UISprite[] sprites = Resources.FindObjectsOfTypeAll(typeof(UISprite)) as UISprite[];

		foreach (UISprite sp in sprites)
		{
			if (sp.atlas == this)
			{
				sp.atlas = null;
				sp.atlas = this;
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(sp);
#endif
			}
		}

		UIFont[] fonts = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];

		foreach (UIFont font in fonts)
		{
			if (font.atlas == this)
			{
				font.atlas = null;
				font.atlas = this;
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(font);
#endif
			}
		}

		UILabel[] labels = Resources.FindObjectsOfTypeAll(typeof(UILabel)) as UILabel[];

		foreach (UILabel lbl in labels)
		{
			if (lbl.font != null && lbl.font.atlas == this)
			{
				UIBaseFont font = lbl.font;
				lbl.font = null;
				lbl.font = font;
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(lbl);
#endif
			}
		}
	}
}