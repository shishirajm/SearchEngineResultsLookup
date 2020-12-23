import React, { useState as useStateMock }  from "react";
import { mount } from 'enzyme';
import Home from './index';



describe('Home', () => {
    const home = mount(<Home />);

      // Loading tests
    it('should render two input fields', () => {
        expect(home.find('input').length).toBe(2);
    });

    it('should render one select fields', () => {
        expect(home.find('select').length).toBe(1);
    });

    it('should render label fields upfront', () => {
        expect(home.find('h2').length).toBe(4);
    });

    it('should not render any result upfront', () => {
        expect(home.find('h3').length).toBe(0);
    });

    // Props test
    it('inputs should load with placeholder text', () => {
        expect(home.find('input#keyword').getElement().props.value).toBe('e-settlements');
        expect(home.find('input#url').getElement().props.value).toBe('www.sympli.com.au');
    });
    
});

