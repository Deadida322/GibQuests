from sklearn.feature_extraction.text import CountVectorizer, TfidfVectorizer
import nltk

nltk.download('stopwords')
from nltk.corpus import stopwords

import pymorphy2
morph = pymorphy2.MorphAnalyzer()

stop_words_ru = list(set(stopwords.words('russian')))
my_stop_words = list(['ооо', 'кася', '.', ',', '\'', '(', ')', '-', '«', '»', '?', '!', ':', ';', '—'])
stop_words = stop_words_ru + my_stop_words

countVectorizer = CountVectorizer(ngram_range=(1, 1))
tfidf_vectorizer = TfidfVectorizer(ngram_range=(1, 1))