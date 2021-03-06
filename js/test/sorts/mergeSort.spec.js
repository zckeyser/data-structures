describe('Merge Sort', function() {
  it('should correctly sort an array', function() {
    var mergeSort = new MergeSort();
    var arrayToSort = util.randomArray();

    // to safeguard against side-effects causing false positives
    expect(util.isSorted(arrayToSort)).toBe(false);

    var result = mergeSort.sort(arrayToSort);

    expect(util.isSorted(result)).toBe(true);
  })
})
