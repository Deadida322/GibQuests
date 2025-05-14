from sklearn.metrics import (accuracy_score)
def print_metrics(y_test, predict):
    print('accuracy_score', round(accuracy_score(y_test, predict), 2))
