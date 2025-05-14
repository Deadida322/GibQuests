def get_vocab(texts_pd):
    vocabulary = set()
    texts_pd["text"].str.split().apply(vocabulary.update)
    vocabulary_size = len(vocabulary)
    return vocabulary_size

def get_text_stats(texts_pd):
    print(texts_pd["text"].apply(lambda x: len(x.split(" "))).describe())

def print_classes(texts_pd):
    print(texts_pd['rating'].value_counts())