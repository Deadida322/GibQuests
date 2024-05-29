from sklearn.feature_extraction.text import CountVectorizer, TfidfVectorizer
import nltk

nltk.download('stopwords')
from nltk.corpus import stopwords

import pymorphy2
morph = pymorphy2.MorphAnalyzer()

stop_words_ru = list(set(stopwords.words('russian')))
my_stop_words = list(['ооо', 'кася', 'то', 'еще', 'это'])
stop_words = stop_words_ru + my_stop_words

countVectorizer = CountVectorizer(stop_words=stop_words, ngram_range=(1, 3))
tfidf_vectorizer = TfidfVectorizer(stop_words=stop_words)