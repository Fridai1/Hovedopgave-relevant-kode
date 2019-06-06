import gspread
import datetime
from googleapiclient.discovery import build
from oauth2client.service_account import ServiceAccountCredentials


scope = ['https://www.googleapis.com/auth/documents']
sheetscope = ['https://www.googleapis.com/auth/spreadsheets']
creds = ServiceAccountCredentials.from_json_keyfile_name('creds.json', scope)
Sheetcred = ServiceAccountCredentials.from_json_keyfile_name('creds.json', sheetscope)
client = gspread.authorize(creds)

DOCUMENT_ID = '1UG92B1-TscV_YCfE6yq2zb07-unQKRu5W0x1apI5UdQ'
SHEET_ID = '18dNxu4CGJag_CJdhP0RRuglsCqj4y8vYyjCxrlZXlcI'

service = build('docs', 'v1', credentials=creds)
Sheetservice = build('sheets', 'v4', credentials=Sheetcred)
document = service.documents().get(documentId=DOCUMENT_ID).execute()

print('The title of the document is: {}'.format(document.get('title')))


def read_paragraph_element(element):
    text_run = element.get('textRun')
    if not text_run:
        return ''
    return text_run.get('content')

def read_strucutural_elements(elements):
    text = ''
    for value in elements:
        if 'paragraph' in value:
            elements = value.get('paragraph').get('elements')
            for elem in elements:
                text += read_paragraph_element(elem)
        elif 'table' in value:
            # The text in table cells are in nested Structural Elements and tables may be
            # nested.
            table = value.get('table')
            for row in table.get('tableRows'):
                cells = row.get('tableCells')
                for cell in cells:
                    text += read_strucutural_elements(cell.get('content'))
        #elif 'tableOfContents' in value:
            # The text in the TOC is also in a Structural Element.
          #  toc = value.get('tableOfContents')
           # text += read_strucutural_elements(toc.get('content'))
    return text


doc_content = document.get('body').get('content')

ActualContent = read_strucutural_elements(doc_content)

#print(read_strucutural_elements(doc_content))
CurrentLength = len(ActualContent)
print(CurrentLength)

date = datetime.datetime.now()

datestr = '{0.day}/{0.month}/{0.year}'.format(date)
values = [
    [CurrentLength, datestr],
    
    # Additional rows ...
]


body = {
    'values': values
   
}
value_input_option = 'RAW'

range_name = 'Sheet1!A2:B31'


result = Sheetservice.spreadsheets().values().append(spreadsheetId=SHEET_ID, range=range_name,insertDataOption='INSERT_ROWS', valueInputOption=value_input_option, body=body).execute()

print('{0} cells updated.'.format(result.get('updatedCells')))