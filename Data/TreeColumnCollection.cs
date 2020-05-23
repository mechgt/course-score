using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace CourseScore.Data
{
    public class TreeColumnCollection : CollectionBase
    {
        public event CollectionChangeEventHandler CollectionChanged;

        public TreeColumnCollection() { }

        public TreeColumn this[int index]
        {
            get { return (TreeColumn)this.List[index]; }
            set { this.List[index] = value; }
        }

        public int Count
        {
            get { return this.List.Count; }
        }

        public void Add(TreeColumn treeColumn)
        {
            this.List.Add(treeColumn);

            if (CollectionChanged != null)
            {
                CollectionChanged.Invoke(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, treeColumn));
            }
        }

        public void AddRange(TreeColumnCollection list)
        {
            this.InnerList.AddRange(list);

            if (CollectionChanged != null)
            {
                CollectionChanged.Invoke(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, list));
            }
        }

        public void Remove(TreeColumn treeColumn)
        {
            this.List.Remove(treeColumn);

            if (CollectionChanged != null)
            {
                CollectionChanged.Invoke(this, new CollectionChangeEventArgs(CollectionChangeAction.Remove, treeColumn));
            }
        }

        public void Sort()
        {
            this.InnerList.Sort();
        }

        public bool Contains(object value)
        {
            return this.List.Contains(value);
        }

        public int IndexOf(TreeColumn treeColumn)
        {
            return List.IndexOf(treeColumn);
        }

        protected override void OnValidate(object value)
        {
            base.OnValidate(value);
            if (!(value is TreeColumn))
            {
                throw new ArgumentException("Collection only supports TreeColumn objects");
            }
        }
    }
}
