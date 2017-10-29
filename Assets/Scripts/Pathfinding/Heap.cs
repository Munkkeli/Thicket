using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding {
  public class Heap<T> where T : IHeapItem<T> {
    T[] items;
    int count;

    public Heap(int size) {
      items = new T[size];
    }

    public void Add(T item) {
      item.Index = count;
      items[count] = item;
      SortUp(item);
      count++;
    }

    public T RemoveFirst() {
      T item = items[0];
      count--;
      items[0] = items[count];
      items[0].Index = 0;
      SortDown(items[0]);
      return item;
    }

    public int Count {
      get { return count; }
    }

    public bool Contains(T item) {
      return Equals(items[item.Index], item);
    }

    public void Update(T item) {
      SortUp(item);
    }

    void SortUp(T item) {
      int up = (item.Index - 1) / 2;
      while (true) {
        T parent = items[up];
        if (item.CompareTo(parent) > 0) {
          Swap(item, parent);
        } else {
          break;
        }

        up = (item.Index - 1) / 2;
      }
    }

    void SortDown(T item) {
      while (true) {
        int left = item.Index * 2 + 1;
        int right = item.Index * 2 + 2;
        int swap = 0;

        if (left < count) {
          swap = left;

          if (right < count && items[left].CompareTo(items[right]) < 0) {
            swap = right;
          }

          if (item.CompareTo(items[swap]) < 0) {
            Swap(item, items[swap]);
          } else {
            return;
          }
        } else {
          return;
        }
      }
    }

    void Swap(T a, T b) {
      items[a.Index] = b;
      items[b.Index] = a;

      int index = a.Index;
      a.Index = b.Index;
      b.Index = index;
    }
  }

  public interface IHeapItem<T> : IComparable<T> {
    int Index { get; set; }
  }
}