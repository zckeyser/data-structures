package sorts

import "../testhelper"

import "testing"

func TestIntMergesort(t *testing.T) {
  arr := testhelper.RandomIntSlice(100)

  sorted := Mergesort(arr)

  testhelper.TestIsSorted(sorted, t)
}

func TestInterfaceMergesort(t *testing.T) {

}
