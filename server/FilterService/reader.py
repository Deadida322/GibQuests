import codecs
import pandas as pd

def get_texts(text_len = 3000):
    data = pd.read_csv('C:/Users/Timovey/Study/2 сем/Магистерская/rus-text/description.csv', delimiter=';',
                names=['file', 'name', 'author', 'rating', 'description'])

    previews = []
    for row in data[['file', 'rating']].head(text_len).itertuples():
        fileObj = codecs.open('C:/Users/Timovey/Study/2 сем/Магистерская/rus-text/previews/' + row.file, "r", "utf_8_sig" )
        text = fileObj.read()
        rating = row.rating
        previews.append((text, rating))
        fileObj.close()
    print(len(previews))
    return previews

