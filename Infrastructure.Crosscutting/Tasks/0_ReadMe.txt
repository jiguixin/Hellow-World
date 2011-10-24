
Tasks 功能目录

该目录下面的类，主要是为了完成程序的后台任务。

一般是功能配置文件的方式记录
<NopConfig>
    <DynamicDiscovery Enabled="true" />
    <Engine Type="" />
    <ScheduleTasks>
      <!--run each minute: 60*1=60-->
      <Thread seconds="60">
        <task name="Emails" type="Nop.Services.Messages.QueuedMessagesSendTask, Nop.Services" enabled="true" stopOnError="false" maxTries="5" />
      </Thread>
      <!--run each 15 minutes: 60*15=900-->
      <Thread seconds="900">
        <task name="UpdateExchangeRates" type="Nop.Services.Directory.UpdateExchangeRateTask, Nop.Services" enabled="true" stopOnError="false" />
      </Thread>
      <!--run each hour: 60*60=3600-->
      <Thread seconds="3600">
        <task name="DeleteGuestsTask" type="Nop.Services.Customers.DeleteGuestsTask, Nop.Services" enabled="true" stopOnError="false" olderThanMinutes="1440" />
      </Thread>
    </ScheduleTasks>
    <Themes basePath="~/Themes/" />
  </NopConfig>
 
