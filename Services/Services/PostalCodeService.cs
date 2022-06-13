using Services.Models;
using Services.Models.Requests;
using Services.Models.Responses;
using Services.Util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PostalCodeService
    {
        // it can be configure as environment variable

        //returns the api data for details
        public async static Task<PostalCodeDetail> GetPostalCodeDetails(SearchParameter searchParam)
        {
            var detail = await FetchPostalCodeDetails(searchParam.PostalCode, searchParam.Url);
            if (detail.Status == (int)HttpStatusCode.OK)
            {
                var res = detail.result;
                var area = GetArea(res.latitude);
                var data = new PostalCodeDetail
                {
                    AdminDistrict = res.admin_district,
                    Country = res.country,
                    ParliamentaryConstituency = res.parliamentary_constituency,
                    Region = Convert.ToString(res.region),
                    Area = area
                };
                return data;
            }
            return null;
        }

        //returns the api data for postal dodes
        public async static Task<List<PostalCode>> GetPostalCode(SearchParameter searchParam)
        {
            List<PostalCode> postalCodes = new List<PostalCode>();
            var result = await FetchPostalCodeAutoComplete(searchParam);

            if (result.Status == (int)HttpStatusCode.OK)
            {
                var payload = result.result;
                //merging area with potal code
                int count = 1;
                foreach (var res in payload)
                {

                    var postalCode = new PostalCode() { Code = res };

                    var detail = await FetchPostalCodeDetails(res, searchParam.Url);
                    if (detail.Status == (int)HttpStatusCode.OK)
                    {
                        postalCode.Area = GetArea(detail.result.latitude);
                    }
                    postalCodes.Add(postalCode);

                    count++;
                    //return if reaches the limit
                    if (count > searchParam.MaxLimit)
                        return postalCodes;
                }
            }

            return postalCodes;
        }

        #region this section can be sifted to other file or interface for loose compling
        private async static Task<PostalCodeRespnose> FetchPostalCodeAutoComplete(SearchParameter searchParam)
        {

            Request request = new Request();
            request.Url = searchParam.Url + string.Format("/postcodes/{0}/autocomplete", searchParam.PostalCode);
            var result = await HttpClientWrapper.GetAsync<PostalCodeRespnose>(request);
            return result;
        }

        private async static Task<PostalCodeDetailResponse> FetchPostalCodeDetails(string postalCode, string Url)
        {
            Request request = new Request();

            request.Url = Url + string.Format("/postcodes/{0}", postalCode);
            var result = await HttpClientWrapper.GetAsync<PostalCodeDetailResponse>(request);
            return result;
        }
        private static string GetArea(double latitude)
        {
            string area = "Midlands";
            var minLatitude = 52.229466;
            var maxLatitude = 53.27169;
            if (latitude < minLatitude)
                area = "South";
            else if (latitude >= maxLatitude)
                area = "North";
            else
                area = "Midlands";
            return area;
        }
        #endregion

    }


}
