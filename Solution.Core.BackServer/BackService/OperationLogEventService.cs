

using Solution.Core.Domain.INpgSqlService;

namespace Solution.Core.BackServer.BackService
{
    public class OperationLogEventService
    {
        private readonly IOperationLogService _operationLogService;
        private readonly IEmployeeService _employeeService;
        private readonly IDictionaryService _dictionaryService;

        public OperationLogEventService(IDictionaryService dictionaryService, IEmployeeService employeeService, IOperationLogService operationLogService)
        {
            _dictionaryService = dictionaryService;
            _employeeService = employeeService;
            _operationLogService = operationLogService;
        }

        //private readonly IServiceScopeFactory _service;
        //private IServiceScope _serviceScope;

        public async void AddLog(long empId, string path, string operName, string requestStr, string responseStr)
        {
            var emp = await _employeeService.GetEmployeeByIdAsync(empId);
            AddEditOperationLogModel model = new AddEditOperationLogModel
            { 
                EmpId=empId,
                EmpName=emp.RealName,
                OperationName=operName,
                ApiPath=path,
                RequestMessage=requestStr,
                ResponseMessage=responseStr
            };
            var tuple = await _operationLogService.AddOperationLogAsync(model);
            if (tuple.Item1 > 0)
            {
                var res = await _dictionaryService.GetDictionaryByPropAsync(new DictionaryQueryModel { Key= "OperationName" });
                if (res != null)
                {
                    bool exists = await _dictionaryService.AnyAsync(operName, CancellationToken.None);
                    if (!exists)
                    {
                        AddEditDictionaryModel dictionary = new AddEditDictionaryModel 
                        {
                            Description= operName,
                            Key= operName,
                            Value= operName,
                            Type= Convert.ToInt32(res.Id)
                        };
                        await _dictionaryService.AddDictionaryAsync(dictionary);
                    }
                }

            }
        }
    }
}
