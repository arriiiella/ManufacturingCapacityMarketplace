export interface SearchCriteria {
    isPageLoad: boolean;
    filter: string;
}

export interface SearchCriteriaSpareCapacity {
    isPageLoad: boolean;
    industryId: number;
    processId: number;
    city: string;
    startDate: string;
    endDate: string;
}