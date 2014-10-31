using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BattleFox {
	public class TreeNode
	{
		public object Data;
		public List<TreeNode> Children;
		public TreeNode Parent;
		public bool IsRoot;

		public TreeNode(List<TreeNode> children, TreeNode parent, bool isroot)
		{
			Children = children;
			Parent = parent;
			IsRoot = isroot;
		}
		public TreeNode(TreeNode parent, bool isroot)
		{
			Children = new List<TreeNode> ();
			Parent = parent;
			IsRoot = isroot;
		}
		public TreeNode()
		{
			Children = new List<TreeNode> ();
			Parent = null;
			IsRoot = true;
		}

		public TreeNode GetChildWithData(object data)
		{
			foreach(TreeNode n in Children)
			{
				if (n.Data.Equals(data))
					return n;
			}
			return null;
		}
	}

	public class Tree {
		public TreeNode RootNode;
	}
}
