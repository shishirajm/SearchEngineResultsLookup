import React, { ReactElement, useState, useEffect } from 'react';

import { Container, Wrapper, Section, Title, Label, Input, Select, Result, Button, Div} from './styles';

export default function Home(): ReactElement {

  const initialState: number[] = [];
  const searchEngines: string[] = ['Google', 'Bing'];

  const [keyWord, setKeyWord] = useState('e-settlements');
  const [url, setUrl] = useState('www.sympli.com.au');
  const [searchEngine, setSearchEngine] = useState(searchEngines[0]);
  const [result, setResult] = useState(initialState);
  const [fetched, setFetched] = useState(false);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false); 

  useEffect(() => {
    document.title = `${searchEngine} Lookup`;
  });

  async function handleLookup() {
    
    setLoading(true);
    setFetched(false);
    setError('');
    const completeUrl = `api/lookup?keyWord=${encodeURI(keyWord)}&url=${encodeURI(url)}&searchEngine=${searchEngine}`;

    try {
      const response = await fetch(completeUrl);
      if (response.ok) {
        const data = await response.json();
        setResult(data);
        setFetched(true);
      } else {
        setError(`Error: ${response.statusText}`);
      }
    } catch (ex) {
      console.log();
      setError('Error retrieving results. Check inputs.');
    }
    setLoading(false);
  }

  return (
    <Container>
      <Wrapper>
        <Section>
          <Title>Lookup Search Engine</Title>
          <Div>
            <Label>Keyword:</Label>
            <Input
                value={keyWord}
                onChange={(e): void => setKeyWord(e.target.value)}
                type="text"
                id="keyWord"
                placeholder="e-settlements"
              />
            </Div>
            <Div>
              <Label>Url to lookup:</Label>
                <Input
                  value={url}
                  onChange={(e): void => setUrl(e.target.value)}
                  type="text"
                  id="url"
                  placeholder="www.sympli.com.au"
                />
          </Div>
          <Div>
            <Label>Search Engine:</Label>
            <Select name="searchEngine" value={searchEngine} onChange={(e) => {setSearchEngine(e.target.value)}}>
              {searchEngines.map(o => <option key={o} value={o}>{o}</option>)}
            </Select>
          </Div>
          <Div><Button onClick={async() => {await handleLookup();}}>Lookup</Button></Div>
        </Section>
      </Wrapper>
      <Wrapper>
        <Section>
          <Label>Search Results:</Label>
          {fetched &&
          <Div>
            <Result><b>{url}</b> found in <b>{result.length}</b> places.</Result>
            <Result>
              Position of Result: {result.join(', ')}
            </Result>
          </Div>
          } 
          { error !== '' &&
            <Result>{error}</Result>
          }
          {
            loading && <Result>Finding the {url} in {keyWord} search on {searchEngine}.</Result>
          }
        </Section>
      </Wrapper>
    </Container>
  );
}
