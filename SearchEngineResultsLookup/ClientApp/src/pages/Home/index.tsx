import React, { ReactElement, useState, useEffect } from 'react';

import { Container, Header} from './styles';

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
      <Header>
        <h2>Lookup Search Engine</h2>
        <div>
          Keyword:
          <input
              value={keyWord}
              onChange={(e): void => setKeyWord(e.target.value)}
              type="text"
              id="keyWord"
              placeholder="e-settlements"
            />
          </div>
          <div>
            Url to lookup:
            <input
              value={url}
              onChange={(e): void => setUrl(e.target.value)}
              type="text"
              id="url"
              placeholder="www.sympli.com.au"
            />
        </div>
        <div>
          Search Engine:
          <select name="searchEngine" value={searchEngine} onChange={(e) => {setSearchEngine(e.target.value)}}>
            {searchEngines.map(o => <option key={o} value={o}>{o}</option>)}
          </select>
        </div>
        <div><a href="/" onClick={(e): void => handleLookup(e)}>Lookup</a></div>
      </Header>
      {fetched &&
      <div>
        <p><b>{url}</b> found in <b>{result.length}</b> places.</p>
        <p>
          Position of Result: {result.join(', ')}
        </p>
      </div>
      } 
      { error !== '' &&
        <div>{error}</div>
      }
      {
        loading && <div>Finding the {url} in {keyWord} search on {searchEngine}.</div>
      }
    </Container>
  );
}
