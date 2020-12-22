import React, { ReactElement, useState, useEffect } from 'react';

import { Container, Wrapper, Section, Title, Label, Input, Select, Result} from './styles';

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

  function handleLookup(e: React.MouseEvent<HTMLAnchorElement, MouseEvent>): void {
    e.preventDefault();
    
    setLoading(true);
    setFetched(false);
    setError('');
    const completeUrl = `api/lookup?keyWord=${encodeURI(keyWord)}&url=${encodeURI(url)}&searchEngine=${searchEngine}`;

    fetch(completeUrl)
      .then(response => response.json() as Promise<number[]>)    
      .then(data => {
        setResult(data);
        setFetched(true);
      })
      .catch(errorMsg => {
        setError('Error retrieving results. Check inputs.');
      })
      .finally(() => {
        setLoading(false);
      });
  }

  return (
    <Container>
      <Wrapper>
        <Section>
          <Title>Lookup Search Engine</Title>
          <div>
            <Label>Keyword:</Label>
            <Input
                value={keyWord}
                onChange={(e): void => setKeyWord(e.target.value)}
                type="text"
                id="keyWord"
                placeholder="e-settlements"
              />
            </div>
            <div>
            <Label>Url to lookup:</Label>
              <Input
                value={url}
                onChange={(e): void => setUrl(e.target.value)}
                type="text"
                id="url"
                placeholder="www.sympli.com.au"
              />
          </div>
          <div>
          <Label>Search Engine:</Label>
            <Select name="searchEngine" value={searchEngine} onChange={(e) => {setSearchEngine(e.target.value)}}>
              {searchEngines.map(o => <option key={o} value={o}>{o}</option>)}
            </Select>
          </div>
          <div><a href="/" onClick={(e): void => handleLookup(e)}>Lookup</a></div>
        </Section>
      </Wrapper>
      <Wrapper>
        <Section>
          <Label>Search Results:</Label>
          {fetched &&
          <div>
            <Result><b>{url}</b> found in <b>{result.length}</b> places.</Result>
            <Result>
              Position of Result: {result.join(', ')}
            </Result>
          </div>
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
