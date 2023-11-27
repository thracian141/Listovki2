export const load = ({ fetch, params}) => {
    console.log('params.listovkaId:', params.listovkaId);
    
    const fetchTest = async () => {
        const data = params.listovkaId;
        return data;
    }

    return {
        test: fetchTest()
    }
}