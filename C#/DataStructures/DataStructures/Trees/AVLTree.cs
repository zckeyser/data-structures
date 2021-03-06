﻿using System;
using DataStructures.trees;

namespace DataStructures.trees
{
    /// <summary>
    /// Generic AVL Tree Implementation
    /// 
    /// The AVL Tree is a type of Balanced BST,
    /// so it has search efficiency of O(log(n)),
    /// unlike a normal BST which ranges from
    /// O(log(n)) to O(n)
    /// </summary>
	public class AVLTree<T> : IBinaryTree<T> where T : IComparable
	{
	    private int count;
		private int height;
		private AVLTree<T> left;
		private AVLTree<T> right;
		private readonly AVLTree<T> parent;

		public IBinaryTree<T> Left { get { return left; } }
		public IBinaryTree<T> Right { get { return right; } }
		public T Value { get; private set; }

	    public AVLTree(AVLTree<T> parent, T value)
		{
			this.parent = parent;
			this.Value = value;
			count = 1;
			height = parent != null ? parent.height + 1 : 1;
		}

		#region Modified BST Methods
		public AVLTree<T> Insert(T toInsert)
		{
            var curr = BSTInsert(toInsert);
            var newKey = curr.Value;

            // check up the parent chain to see if we got unbalanced somewhere
            while(curr.parent != null && System.Math.Abs(curr.BalanceFactor()) <= 1)
            {
                curr = curr.parent;
            }

            // if we're unbalanced, fix it
            var balanceFactor = curr.BalanceFactor();
            if(balanceFactor > 1)
            {
                // we're biased towards the right -- 
                // we'll need a Left-Left or a Left-Right rotation
                if(newKey.CompareTo(curr.Value) < 0)
                {
                    // Left-Left rotation
                    return curr.RotateLeft();
                }
                else // since we don't have duplicate nodes, the unbalanced root must be greater than the new key
                {
                    // Left-Right rotation
                    curr.left = curr.left.RotateLeft();
                    return curr.RotateRight();
                }
            }
            else if(balanceFactor < -1)
            {
                // we're biased towards the left -- 
                // we'll need a Right-Right or a Left-Right rotation
                if (newKey.CompareTo(curr.Value) > 0)
                {
                    // Right-Left rotation
                    return curr.RotateRight();
                }
                else // since we don't have duplicate nodes, the unbalanced root must be less than the new key
                {
                    // Right-Left rotation
                    curr.right = curr.right.RotateRight();
                    return curr.RotateRight();
                }
            }

            return curr;
		}

        private AVLTree<T> BSTInsert(T toInsert)
        {
            var compareValue = toInsert.CompareTo(Value);

            if (compareValue > 0)
            {
                // toInsert > value
                // put it in the right subtree
                if (right == null)
                {
                    // if we don't have a right subtree,
                    // make one starting with toInsert
                    right = new AVLTree<T>(this, toInsert);

                    return right;
                }
                else
                {
                    // if we have a right subtree, insert into that
                    return right.BSTInsert(toInsert);
                }
            }
            else if (compareValue < 0)
            {
                // toInsert < value
                // put it in the left subtree
                if (left == null)
                {
                    // if we don't have a left subtree,
                    // make one starting with toInsert
                    left = new AVLTree<T>(this, toInsert);

                    return left;
                }
                else
                {
                    // if we have a left subtree, insert into that
                    return left.BSTInsert(toInsert);
                }
            }
            else
            {
                // toInsert == value
                // if we already have an instance of toInsert in the tree,
                // increment the counter for the value's node
                count++;
                return this;
            }
        }

		public void Remove(T toRemove)
		{
			if (!Contains(toRemove))
			{
				throw new ArgumentException(string.Format("Value {0} attempted to be removed from a tree which does not contain it", toRemove));
			}

			var compareVal = toRemove.CompareTo(Value);

			if (compareVal < 0)
			{
				// toRemove < value
				// the value must be in the left subtree -- remove it from there
				left.Remove(toRemove);
			}
			else if (compareVal > 0)
			{
				// toRemove > value
				// the value must be in the right subtree -- remove it from there
				right.Remove(toRemove);
			}
			else
			{
				// toRemove is equal to value
				if (count > 1)
				{
					// remove an instance from this node
					count--;
				}
				else
				{
					// remove the current node
					if (IsLeaf())
					{
						// if this is a leaf, we can just remove its reference
						parent.ReplaceChild(Value, null);
					}
					else
					{
						// find the closest descendant
						var desc = left != null ? left.GreatestDescendant() : right.SmallestDescendant();

						// replace this node's values with that node's values, and remove it
						Value = desc.Value;
						count = desc.count;

						// get rid of the old copy of the descendant node
						desc.Remove(desc.Value);
					}
				}
			}
		}
		#endregion

		#region AVL Methods
		private AVLTree<T> RotateLeft()
		{
			// get nodes involved in rotation
			var x = left;
			var T2 = left.right;

			// perform rotation
			left = T2;
			x.right = this;

			// update heights
			x.height = System.Math.Max(x.left.height, x.right.height) + 1;
			T2.height = System.Math.Max(T2.left.height, T2.right.height) + 1;

			return x;
		}

		private AVLTree<T> RotateRight()
		{
			// get nodes involved in rotation
			var y = right;
			var T2 = right.left;

			// perform rotation
			right = T2;
			y.left = this;

			// update heights
			y.height = System.Math.Max(y.left.height, y.right.height) + 1;
			T2.height = System.Math.Max(T2.left.height, T2.right.height) + 1;

			return y;
		}

		private int BalanceFactor()
		{
			var leftHeight = left != null ? left.height : 0;
			var rightHeight = right != null ? right.height : 0;

			return leftHeight - rightHeight;
		}
		#endregion

		#region BST Methods
		private void ReplaceChild(T val, AVLTree<T> replacement)
		{
			if (left != null && left.Value.CompareTo(val) == 0)
				left = replacement;
			else if (right != null && right.Value.CompareTo(val) == 0)
				right = replacement;
			else
				throw new ArgumentException("attempt to replace child that does not exist", "val");
		}

		private bool IsLeaf()
		{
			return left == null && right == null;
		}

		private AVLTree<T> GreatestDescendant()
		{
			return right != null ? right.GreatestDescendant() : this;
		}

		private AVLTree<T> SmallestDescendant()
		{
			return left != null ? left.SmallestDescendant() : this;
		} 

		public bool Contains(T target)
		{
			var compareVal = target.CompareTo(Value);

			if (compareVal == 0)
				return true;
			else if (compareVal < 0 && left != null)
				return left.Contains(target);
			else if (compareVal > 0 && right != null)
				return right.Contains(target);
			else
				return false;
		}
		#endregion
	}
}
