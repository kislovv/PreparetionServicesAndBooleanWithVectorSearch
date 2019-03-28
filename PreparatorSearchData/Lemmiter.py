import pymorphy2
import string
import codecs
import re

morp = pymorphy2.MorphAnalyzer()
filesPath = 'C:/Users/Kirill/source/repos/PreparatorSearchData/PreparatorSearchData/Resources/Site/' 
filesPathLem = 'C:/Users/Kirill/source/repos/PreparatorSearchData/PreparatorSearchData/Resources/Lemmiter/'
for i in range(1,101):
	nameFile = filesPath + str(i) + '.txt'
	outputFileName = filesPathLem + str(i) + '.txt'
	f = codecs.open(nameFile, 'r', 'utf_8_sig')
	data = f.read()
	out = data.translate(str.maketrans('', '', string.punctuation + string.ascii_letters + string.digits + '@' + u"\u00A9")).split()
	f.close()
	w = codecs.open(outputFileName, 'w', 'utf_8_sig')
	for word in out:
		infWord = morp.parse(word)[0]
		if not infWord is None and infWord.normal_form != "—" and infWord.normal_form != "–" :
			w.write(infWord.normal_form+"\r\n")
	w.close()