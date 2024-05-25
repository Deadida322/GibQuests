from nltk.corpus import stopwords
from sklearn.feature_extraction.text import CountVectorizer

stop_words = list(set(stopwords.words('russian')))
vectorizer = CountVectorizer(stop_words=stop_words, ngram_range=(1, 1))