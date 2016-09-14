#ifndef BINARY_SEARCH_TREE_HEADER
#define BINARY_SEARCH_TREE_HEADER

typedef struct BinarySearchTree {
    int value;
    int count;
    BinarySearchTree *parent;
    BinarySearchTree *left;
    BinarySearchTree *right;
} BinarySearchTree;

void BinarySearchTree_init(BinarySearchTree *node, BinarySearchTree *parent, int n);
void BinarySearchTree_insert(BinarySearchTree *root, int n);
void BinarySearchTree_remove(BinarySearchTree *root, int n);
int BinarySearchTree_contains(BinarySearchTree *root, int n);
int BinarySearchTree_max(BinarySearchTree *root);
int BinarySerachTree_min(BinarySearchTree *root);

#endif
