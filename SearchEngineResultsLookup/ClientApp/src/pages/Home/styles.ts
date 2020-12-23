import styled, { css } from 'styled-components';

export const Container = styled.div``;

export const Wrapper = styled.section`
  padding: 4em;
  background: #707070;
  @media (max-width: 500px) {
    padding: 0;
  }
`

export const Title = styled.h1`
  font-size: 1.5em;
  text-align: center;
  color: white;
`

export const Label = styled.h2`
  font-size: 1.2em;
  text-align: left;
  color: white;
  width: 25%;
  @media (max-width: 500px) {
    text-align: center;
    width: 100%;
    flex-direction: column;
  }
`

export const Result = styled.h3`
  font-size: 1.1em;
  text-align: left;
  color: white;
  width: 80%;
`

export const Section = styled.header`
  background-color: #909090;
  padding: 30px 40px;
  color: white;
  text-align: center;
  }
`

export const Div = styled.div`
  display: flex;
  align-items: center;
  @media (max-width: 500px) {
    flex-direction: column;
  }
`

export const Input = styled.input`
  padding: 0.5em;
  color: black;
  background: White;
  border: none;
  border-radius: 3px;
  width: 65%;
  font-size: 16px;
  border-radius: 3px;
  @media (max-width: 500px) {
    width: 90%;
    align: center;
  }
`

export const Select = styled.select`
  padding: 0.5em;
  margin: 0.5em;
  font-size: 16px;
  line-height: 1.3;
  width: 66.8%;
  box-sizing: border-box;
  margin: 0;
  border: 1px solid #aaa;
  border-radius: 0;
  @media (max-width: 500px) {
    width: 90%;
    align: center;
  }
`

export const Button = styled.button`
  align-items: center;
  padding: 0.5em;
  margin: 0.5em;
  display: block;
  align-items: center;
  text-decoration: none;
  border: 0;
  width: 90%;
  background: #d9d9d9;
  color: #555;
  text-align: center;
  font-size: 16px;
  cursor: pointer;
  transition: 0.3s;
  border-radius: 0;
  margin: 10px;
  border-radius: 3px;
  @media (max-width: 500px) {
    width: 90%;
    align: center;
  }
  &:hover {
    background: #c9c9c9;
  }
`;
